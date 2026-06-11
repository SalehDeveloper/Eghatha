using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Common.Services;
using Eghatha.Application.Features.Notifications.Dtos;
using Eghatha.Application.Features.Teams.Dtos;
using Eghatha.Application.Features.Teams.Queries.GetTeamDisasters;
using Eghatha.Application.Features.Teams.Queries.GetTeamMemberInfo;
using Eghatha.Application.Features.Volunteers.Dtos;
using Eghatha.Domain.Disasters;
using Eghatha.Domain.Identity;
using Eghatha.Domain.Teams;
using Eghatha.Domain.Teams.Resources;
using Eghatha.Domain.Teams.TeamMembers;
using Eghatha.Domain.Volunteers;
using Eghatha.Domain.Volunteers.Equipments;
using Eghatha.Infastructure.Data;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Repositories
{
    public class TeamRepository : BaseRepository<Team>, ITeamRepository
    {
        private readonly ITeamOperationalLocationProvider _locationProvider;

        public TeamRepository(AppDbContext context, ITeamOperationalLocationProvider locationProvider) : base(context)
        {
            _locationProvider = locationProvider;
        }


        public async Task<List<Team>> GetTeamsByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
        {
            return await _context.Set<Team>().Where(t => ids.Contains(t.Id)).ToListAsync(cancellationToken);
        }
        public async Task<Team?> GetTeamForAUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Set<Team>().Include(t => t.Members).FirstOrDefaultAsync(t => t.Members.Any(tm => tm.UserId == userId), cancellationToken);
        }

        public async Task<Team?> GetTeamByIdWithMembersAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<Team>().Include(t => t.Members).FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        }

        public async Task<Team?> GetTeamByIdWithResourcesAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<Team>().Include(t => t.Resources).FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        }

        public async Task AddTeamMemberAsync(TeamMember member, CancellationToken cancellationToken)
        {
            await _context.Set<TeamMember>().AddAsync(member, cancellationToken);
        }

        public async Task AddTeamResourceAsync(Resource resource, CancellationToken cancellationToken)
        {
            await _context.Set<Resource>().AddAsync(resource, cancellationToken);
        }

        public async Task<PaginatedList<TeamDto>> GetTeamsAsync(int page, int pageSize, string? searchTerm, string? status, string? speciality, string? province, string? city, CancellationToken cancellationToken)
        {
            var query = _context.Set<Team>().Include(t => t.Members).AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var st = searchTerm.Trim();
                query = query.Where(t => EF.Functions.Like(t.Name, $"%{st}%"));
            }

            TeamStatus? teamStatus = null;
            TeamSpeciality? teamSpeciality = null;

            if (!string.IsNullOrWhiteSpace(status))
            {
                teamStatus = TeamStatus.FromName(status, true);
                query = query.Where(t => t.Status == teamStatus);
            }



            if (!string.IsNullOrWhiteSpace(speciality))
            {
                teamSpeciality = TeamSpeciality.FromName(speciality, true);
                query = query.Where(t => t.Speciality == teamSpeciality);
            }

            if (!string.IsNullOrWhiteSpace(province))
            {
                var pr = province.Trim();

                query = query.Where(t => EF.Functions.Like(t.Province, $"%{pr}%"));

            }
            if (!string.IsNullOrWhiteSpace(city))
            {
                var ci = city.Trim();

                query = query.Where(t => EF.Functions.Like(t.City, $"%{ci}%"));

            }

            var totalCount = await query.CountAsync(cancellationToken);

            var teams = await query
          .OrderByDescending(t => t.CreatedAt)
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .Select(t => new
          {
              t.Id,
              t.Name,
              Speciality = t.Speciality.Name,
              t.Province,
              t.City,
              Status = t.Status.Name,

              Members = t.Members.Select(m => new
              {
                  m.UserId,
                  m.IsLeader,
                  m.Status
              })
          })
          .ToListAsync(cancellationToken);


            var userIds = teams
                .SelectMany(t => t.Members)
                .Select(m => m.UserId)
                .Distinct()
                .ToList();

            var users = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName
                })
                .ToDictionaryAsync(u => u.Id, cancellationToken);


            var items = teams.Select(t =>
            {
                var leader = t.Members.FirstOrDefault(m => m.IsLeader);

                string? leaderName = null;

                if (leader != null && users.TryGetValue(leader.UserId, out var user))
                {
                    leaderName = $"{user.FirstName} {user.LastName}";
                }

                var membersCount = t.Members.Count();
                var activeMembersCount = t.Members.Count(m => m.Status == TeamMemberStatus.Active);

                var isReady =
                    t.Status == TeamStatus.Active.Name &&
                    activeMembersCount > 0;

                return new TeamDto(
                    t.Id,
                    t.Name,
                    t.Speciality,
                    t.Province,
                    t.City,
                    t.Status,
                    leaderName,
                    membersCount,
                    activeMembersCount,
                    isReady
                );
            }).ToList();


            return new PaginatedList<TeamDto>
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Items = items
            };



        }
        public async Task<TeamDto?> GetTeamOverviewAsync(Guid teamId, CancellationToken cancellationToken)
        {
            var team = await _context.Set<Team>()
                .AsNoTracking()
                .Where(t => t.Id == teamId)
                .Select(t => new
                {
                    t.Id,
                    t.Name,
                    Speciality = t.Speciality.Name,
                    t.Province,
                    t.City,
                    Status = t.Status.Name,
                    Members = t.Members.Select(m => new
                    {
                        m.UserId,
                        m.IsLeader,
                        m.Status
                    })
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (team is null)
                return null;

            var leader = team.Members.FirstOrDefault(m => m.IsLeader);

            string? leaderName = null;

            if (leader != null)
            {
                var user = await _context.Users
                    .Where(u => u.Id == leader.UserId)
                    .Select(u => new { u.FirstName, u.LastName })
                    .FirstOrDefaultAsync(cancellationToken);

                if (user != null)
                    leaderName = $"{user.FirstName} {user.LastName}";
            }

            var membersCount = team.Members.Count();
            var activeMembersCount = team.Members.Count(m => m.Status == TeamMemberStatus.Active);

            var isReady =
                team.Status == TeamStatus.Active.Name &&
                activeMembersCount > 0;

            return new TeamDto(
                team.Id,
                team.Name,
                team.Speciality,
                team.Province,
                team.City,
                team.Status,
                leaderName,
                membersCount,
                activeMembersCount,
                isReady
            );
        }
        public async Task<PaginatedList<TeamMemberDto>> GetTeamMembersAsync(Guid teamId, int page, int pageSize, string? searchTerm, string? status, CancellationToken cancellationToken)
        {
            var query = _context.Set<TeamMember>()
             .AsNoTracking()
              .Where(m => EF.Property<Guid>(m, "TeamId") == teamId);



            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var st = searchTerm.Trim();
                query = query.Where(m => EF.Functions.Like(m.Id.ToString(), $"%{st}%"));
            }

            TeamMemberStatus? memberStatus = null;

            if (!string.IsNullOrWhiteSpace(status))
            {
                memberStatus = TeamMemberStatus.FromName(status, true);

                query = query.Where(m => m.Status == memberStatus);
            }



            var totalCount = await query.CountAsync(cancellationToken);

            var members = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new
                {
                    m.Id,
                    m.UserId,
                    m.JobTitle,
                    Status = m.Status.Name,
                    m.IsLeader

                })
                .ToListAsync(cancellationToken);

            var userIds = members.Select(m => m.UserId).ToList();

            var users = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, cancellationToken);

            var items = members.Select(m =>
            {
                var user = users[m.UserId];
                return new TeamMemberDto(
                    m.Id,
                    $"{user.FirstName} {user.LastName}",
                    m.JobTitle,
                    m.Status,
                    m.IsLeader,
                    user.PhotoUrl
                );
            }).ToList();

            return new PaginatedList<TeamMemberDto>
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Items = items
            };
        }
        public async Task<PaginatedList<TeamResourceDto>> GetTeamResourcesAsync(Guid teamId, int page, int pageSize, string? type, CancellationToken cancellationToken)
        {
            var query = _context.Set<Resource>()
              .AsNoTracking()
               .Where(m => EF.Property<Guid>(m, "TeamId") == teamId);


            ResourceType? resourceType = null;

            if (!string.IsNullOrWhiteSpace(type))
                resourceType = ResourceType.FromName(type, true);

            if (type != null)
                query = query.Where(r => r.Type == resourceType);

            var totalCount = await query.CountAsync(cancellationToken);

            var resources = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new TeamResourceDto(
                    r.Id,
                    r.Type.Name,
                    r.Quantity,
                    r.Status.Name,
                    r.Type.IsConsumable
                ))
                .ToListAsync(cancellationToken);

            return new PaginatedList<TeamResourceDto>
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Items = resources
            };
        }
        public async Task<IReadOnlyList<Team>> GetAvailableTeamsAsync(IReadOnlyList<TeamSpeciality> specialities, CancellationToken cancellationToken)
        {

            return await _context.Set<Team>().AsNoTracking()
                   .Where(x =>
                       specialities.Contains(x.Speciality)
                       && (x.Status == TeamStatus.Active || x.Status == TeamStatus.Returning)
                       && x.Members.Any(m => m.Status == TeamMemberStatus.Active)
                   )
                   .ToListAsync(cancellationToken);
        }
        public async Task<Guid?> GetTeamLeaderByUserId(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Set<TeamMember>().AsNoTracking()
                .Where(x => x.UserId == userId && x.IsLeader)
                .Select(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<TeamMemberInfo> GetCurrentTeamMemberInfo(Guid userId, CancellationToken cancellationToken)
        {

            var team = await _context.Set<Team>()
                .Include(x => x.Members)
                .Where(t => t.Members.Any(tm => tm.UserId == userId)).FirstOrDefaultAsync(cancellationToken);

            var teamLeader = team.Leader;

            return new TeamMemberInfo(team.Id, teamLeader.UserId == userId);

        }
        public async Task<List<TeamMapDto>> GetTeamsOnMapAsync(CancellationToken cancellationToken)
        {
            var relevantStatuses = new[]
       {
            TeamStatus.Active,
            TeamStatus.OnMission,
            TeamStatus.Returning
        };

            var teams = await _context.Set<Team>()
                .AsNoTracking()
                .Where(t => relevantStatuses.Contains(t.Status))
                .ToListAsync(cancellationToken);

            if (teams.Count == 0)
                return new List<TeamMapDto>();

            // Only look up disaster assignments for OnMission / Returning
            var missionTeamIds = teams
                .Where(t => t.Status == TeamStatus.OnMission || t.Status == TeamStatus.Returning)
                .Select(t => t.Id)
                .ToHashSet();

            var disasterByTeam = missionTeamIds.Count > 0
                ? await _context.Set<DisasterTeam>()
                    .Where(dt => missionTeamIds.Contains(dt.TeamId))
                    .Select(dt => new { dt.TeamId, dt.DisasterId })
                    .ToListAsync(cancellationToken)
                    .ContinueWith(r => r.Result
                        .GroupBy(x => x.TeamId)
                        .ToDictionary(g => g.Key, g => g.First().DisasterId))
                : new Dictionary<Guid, Guid>();

            // Resolve locations in parallel (Redis → DB fallback)
            var locationTasks = teams.Select(async t =>
            {
                var (loc, isLive) = await _locationProvider.GetLocationAsync(t, cancellationToken);
                return (t.Id, loc, isLive);
            });

            var locations = (await Task.WhenAll(locationTasks))
                .ToDictionary(x => x.Id, x => (x.loc, x.isLive));

            var result = teams.Select(t =>
            {
                var (loc, isLive) = locations.TryGetValue(t.Id, out var l)
                    ? l : (t.Location, false);

                disasterByTeam.TryGetValue(t.Id, out var disasterId);

                return new TeamMapDto(
                    t.Id,
                    t.Name,
                    t.Speciality.Name,
                    t.Status.Name,
                    loc?.Latitude ?? 0,
                    loc?.Longitude ?? 0,
                    isLive,
                    missionTeamIds.Contains(t.Id) ? disasterId : null
                );
            }).ToList();

            return result;
        }

        public async Task<PaginatedList<TeamDisastersDto>> GetTeamDisastersAsync(Guid teamId, int page, int pageSize, CancellationToken cancellationToken)
        {
            var query = from dt in _context.Set<DisasterTeam>()
                        join d in _context.Set<Disaster>()
                        on dt.DisasterId equals d.Id
                        where dt.TeamId == teamId
                        orderby d.StartTime
                        select new TeamDisastersDto(dt.DisasterId, d.Title, d.City, d.Province, d.Location.Latitude, d.Location.Longitude, d.Type.Name, d.Status.Name, d.StartTime);


            return new PaginatedList<TeamDisastersDto>
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = await query.CountAsync(cancellationToken),
                TotalPages = (int)Math.Ceiling(await query.CountAsync(cancellationToken) / (double)pageSize),
                Items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken)

            };
        }


        public async Task<TeamDisastersDto> GetTeamDisasterAsync(Guid teamId, CancellationToken cancellationToken)
        {
            var query = from dt in _context.Set<DisasterTeam>()
                        join d in _context.Set<Disaster>()
                        on dt.DisasterId equals d.Id
                        where dt.TeamId == teamId && (d.Status == DisasterStatus.Reported || d.Status == DisasterStatus.InProgress || d.Status == DisasterStatus.Resolved)
                        orderby d.StartTime
                        select new TeamDisastersDto(dt.DisasterId, d.Title, d.City, d.Province, d.Location.Latitude, d.Location.Longitude, d.Type.Name, d.Status.Name, d.StartTime);


            return await query.FirstOrDefaultAsync(cancellationToken);
        }
    }
}
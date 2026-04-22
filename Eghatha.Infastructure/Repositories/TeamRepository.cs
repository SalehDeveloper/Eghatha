using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Teams.Dtos;
using Eghatha.Domain.Identity;
using Eghatha.Domain.Teams;
using Eghatha.Domain.Teams.Resources;
using Eghatha.Domain.Teams.TeamMembers;
using Eghatha.Infastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Repositories
{
    public class TeamRepository : BaseRepository<Team>, ITeamRepository
    {
        public TeamRepository(AppDbContext context) : base(context)
        {
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

        public async Task AddTeamMemberAsync(TeamMember member , CancellationToken  cancellationToken)
        {
            await _context.Set<TeamMember>().AddAsync(member, cancellationToken);
        }

        public async Task AddTeamResourceAsync(Resource resource, CancellationToken cancellationToken)
        {
            await _context.Set<Resource>().AddAsync(resource, cancellationToken);
        }


        public async Task<PaginatedList<TeamDto>> GetTeamsAsync(
            int page,
            int pageSize,
            string? searchTerm,
            TeamStatus? status,
            TeamSpeciality? speciality,
            string? province,
            string? city , 
            CancellationToken cancellationToken )
        {
            var query = _context.Set<Team>().Include(t=>t.Members).AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
               var st = searchTerm.Trim();
                query = query.Where(t => EF.Functions.Like(t.Name, $"%{st}%"));
            }

            if (status != null && !string.IsNullOrWhiteSpace( status.Name))
            {
                query = query.Where(t => t.Status == status);
            }

            if (speciality != null && !string.IsNullOrWhiteSpace( speciality.Name))
            {              
                query = query.Where(t => t.Speciality == speciality);
            }

            if (!string.IsNullOrWhiteSpace(province))
            {
                var pr = province.Trim();

                query = query.Where(t => EF.Functions.Like(t.Province, $"%{pr}%"));
              
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
                    TeamSpeciality.FromName( t.Speciality),
                    t.Province,
                    t.City,
                    TeamStatus.FromName (t.Status),
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




        public async Task<TeamDto?> GetTeamOverviewAsync(
    Guid teamId,
    CancellationToken cancellationToken)
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
                TeamSpeciality.FromName(team.Speciality),
                team.Province,
                team.City,
                TeamStatus.FromName(team.Status),
                leaderName,
                membersCount,
                activeMembersCount,
                isReady
            );
        }


        public async Task<PaginatedList<TeamMemberDto>> GetTeamMembersAsync(
    Guid teamId,
    int page,
    int pageSize,
    string? searchTerm,
    TeamMemberStatus? status,
    CancellationToken cancellationToken)
        {
            var query = _context.Set<TeamMember>()
             .AsNoTracking()
              .Where(m => EF.Property<Guid>(m, "TeamId") == teamId);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var st = searchTerm.Trim();
                query = query.Where(m => EF.Functions.Like(m.Id.ToString(), $"%{st}%"));
            }

            if (status != null)
                query = query.Where(m => m.Status == status);

             


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



        public async Task<PaginatedList<TeamResourceDto>> GetTeamResourcesAsync(
    Guid teamId,
    int page,
    int pageSize,
    ResourceType? type,
    CancellationToken cancellationToken)
        {
            var query = _context.Set<Resource>()
              .AsNoTracking()
               .Where(m => EF.Property<Guid>(m, "TeamId") == teamId);

            if (type != null)
                query = query.Where(r => r.Type == type);

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
    }
}
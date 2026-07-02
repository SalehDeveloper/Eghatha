using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Disasters.Dtos;
using Eghatha.Application.Features.Disasters.Queries.GetDisasterVolunteers;
using Eghatha.Application.Features.Teams.Queries.GetTeamDisasters;
using Eghatha.Domain.Disasters;
using Eghatha.Domain.Disasters.AffectedPersons;
using Eghatha.Domain.Disasters.DisasterResources;
using Eghatha.Domain.Disasters.DisasterVolunteers;
using Eghatha.Domain.Disasters.Reports;
using Eghatha.Domain.Volunteers;
using Eghatha.Infastructure.Data;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;

namespace Eghatha.Infastructure.Repositories
{
    public class DisasterRepository : BaseRepository<Domain.Disasters.Disaster>, IDisasterRepository
    {
        public DisasterRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Disaster> GetByIdWithVolunteersAndResourcesAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<Disaster>()
                .Include(x => x.Resources)
                .Include(x => x.Volunteers)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public async Task<Disaster> GetByIdWithTeamsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<Disaster>().Include(x => x.Teams).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Disaster> GetByIdWithVolunteersAsync(Guid id, CancellationToken cancellationToken)
        {

            return await _context.Set<Disaster>().Include(x => x.Volunteers).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);


        }

        public async Task<Disaster> GetByIdWithReportAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<Disaster>().Include(x => x.Report).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public async Task AddVolunteersAsync(IEnumerable<DisasterVolunteer> volunteers)
        {
            await _context.Set<DisasterVolunteer>()
                .AddRangeAsync(volunteers);
        }

        public async Task AddAffectedPersonsAsync(IEnumerable<AffectedPerson> persons, CancellationToken cancellationToken)
        {
            await _context.Set<AffectedPerson>()
                .AddRangeAsync(persons, cancellationToken);
        }

        public async Task<Disaster> GetByIdWithTeamsAndResources(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<Disaster>().Include(x => x.Teams).Include(x => x.Resources).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Disaster> GetByIdWithResourcesAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<Disaster>().Include(x => x.Resources).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        }

        public async Task<Disaster> GetByIdWithAffectedPersonsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<Disaster>().Include(x => x.AffectedPeople).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Disaster> GetByIdWithAllDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<Disaster>()
                .Include(x => x.Teams)
                .Include(x => x.Volunteers)
                .Include(x => x.Resources)
                .Include(x => x.AffectedPeople)
                .Include(x => x.Report)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task AddResourceAsync(DisasterResource resource, CancellationToken cancellationToken)
        {
            await _context.Set<DisasterResource>().AddAsync(resource, cancellationToken);
        }

        public async Task AddReportAsync(Report report, CancellationToken cancellationToken)
        {
            await _context.Set<Report>().AddAsync(report, cancellationToken);
        }

        public async Task<PaginatedList<DisasterDto>> GetDisastersAsync(int page, int pageSize, string? city, string? province, string? type, string? status, DateTimeOffset? from, DateTimeOffset? to, CancellationToken cancellationToken)
        {
            var query = _context.Set<Disaster>()
                .AsNoTracking()
                .AsQueryable();

            // 🔹 Filters
            if (!string.IsNullOrWhiteSpace(city))
            {
                var c = city.Trim();
                query = query.Where(d => EF.Functions.Like(d.City, $"%{c}%"));
            }

            if (!string.IsNullOrWhiteSpace(province))
            {
                var p = province.Trim();
                query = query.Where(d => EF.Functions.Like(d.Province, $"%{p}%"));
            }



            DisasterType? disasterType = null;
            DisasterStatus? disasterStatus = null;

            if (!string.IsNullOrWhiteSpace(type))
            {
                disasterType = DisasterType.FromName(type, true);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                disasterStatus = DisasterStatus.FromName(status, true);
            }

            if (disasterType is not null)
            {
                query = query.Where(d => d.Type == disasterType);
            }

            if (disasterStatus is not null)
            {
                query = query.Where(d => d.Status == disasterStatus);
            }

            if (from.HasValue)
                query = query.Where(d => d.StartTime >= from.Value);

            if (to.HasValue)
                query = query.Where(d => d.StartTime <= to.Value);


            var totalCount = await query.CountAsync(cancellationToken);


            var items = await query
                .OrderByDescending(d => d.StartTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new DisasterDto(
                    d.Id,
                    d.Title,
                    d.City,
                    d.Province,
                    d.Location.Latitude,
                    d.Location.Longitude,
                    d.Type.Name,
                    d.Status.Name,
                    d.StartTime
                ))
                .ToListAsync(cancellationToken);

            return new PaginatedList<DisasterDto>
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Items = items
            };
        }

        public async Task<PaginatedList<DisasterVolunteerDto>> GetDisasterVolunteersAsync(Guid disasterId, int page, int pageSize, CancellationToken cancellationToken)
        {
            var query =
                         from dv in _context.Set<DisasterVolunteer>()
                         join v in _context.Set<Volunteer>()
                         on dv.VolunteerId equals v.Id
                         join u in _context.Set<ApplicationUser>()
                         on v.UserId equals u.Id  
                         where dv.DisasterId == disasterId
                         select new DisasterVolunteerDto(
                             v.Id,
                             $"{u.FirstName} {u.LastName}",
                             u.Email,
                             u.PhoneNumber,
                             u.PhotoUrl,
                             v.Status.Name);




            return new PaginatedList<DisasterVolunteerDto>
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = await query.CountAsync(cancellationToken),
                TotalPages = (int)Math.Ceiling(await query.CountAsync(cancellationToken) / (double)pageSize),
                Items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken)

            };

        }
    }
}
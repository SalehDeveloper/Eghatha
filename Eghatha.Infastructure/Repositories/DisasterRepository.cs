using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Disasters.Dtos;
using Eghatha.Domain.Disasters;
using Eghatha.Domain.Disasters.AffectedPersons;
using Eghatha.Domain.Disasters.DisasterResources;
using Eghatha.Domain.Disasters.DisasterVolunteers;
using Eghatha.Domain.Disasters.Reports;
using Eghatha.Infastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Eghatha.Infastructure.Repositories
{
    public class DisasterRepository : BaseRepository<Domain.Disasters.Disaster>, IDisasterRepository
    {
        public DisasterRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Disaster> GetByIdWithTeamsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<Disaster>().Include(x => x.Teams).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Disaster> GetByIdWithVolunteersAsync(Guid id, CancellationToken cancellationToken)
        {

            return await _context.Set<Disaster>().Include(x => x.Volunteers).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);


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


        public async Task<PaginatedList<DisasterDto>> GetDisastersAsync(int page, int pageSize, string? city, string? province, DisasterType? type, DisasterStatus? status, DateTimeOffset? from, DateTimeOffset? to, CancellationToken cancellationToken)
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

            if (type != null)
                query = query.Where(d => d.Type == type);

            if (status != null)
                query = query.Where(d => d.Status == status);

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
                    d.Type,
                    d.Status,
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
    }
}
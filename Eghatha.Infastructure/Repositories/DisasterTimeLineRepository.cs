using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Disasters.Dtos;
using Eghatha.Application.Features.Disasters.Queries.GetTimeLine;
using Eghatha.Domain.Disasters;
using Eghatha.Infastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Eghatha.Infastructure.Repositories
{
    public class DisasterTimeLineRepository : BaseRepository<DisasterTimeLineEvent>, IDisasterTimeLineRepository
    {
        public DisasterTimeLineRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PaginatedList<DisasterTimelineDto>> GetTimelineAsync(
    Guid disasterId,
    int page,
    int pageSize,
    string? eventType,
    TimelineSortDirection sort,
    CancellationToken cancellationToken)
        {
            var query = _context.Set<DisasterTimeLineEvent>()
                .AsNoTracking()
                .Where(x => x.DisasterId == disasterId);

            // 🎯 Filter
            if (!string.IsNullOrWhiteSpace(eventType))
            {
                query = query.Where(x => x.EventType == eventType);
            }

        
            query = sort == TimelineSortDirection.Newest
                ? query.OrderByDescending(x => x.OccurredAt)
                : query.OrderBy(x => x.OccurredAt);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new DisasterTimelineDto(
                    x.Id,
                    x.EventType,
                    x.Description,
                    x.OccurredAt
                ))
                .ToListAsync(cancellationToken);

            return new PaginatedList<DisasterTimelineDto>
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
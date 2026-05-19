using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Disasters.Dtos;
using Eghatha.Application.Features.Disasters.Queries.GetTimeLine;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Disasters;

namespace Eghatha.Application.Common.Interfaces
{
    public interface IDisasterTimeLineRepository : IBaseRepository<DisasterTimeLineEvent>
    {
        Task<PaginatedList<DisasterTimelineDto>> GetTimelineAsync(
   Guid disasterId,
   int page,
   int pageSize,
   string? eventType,
   TimelineSortDirection sort,
   CancellationToken cancellationToken);
    }
}

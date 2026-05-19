using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Disasters.Dtos;
using MediatR;

namespace Eghatha.Application.Features.Disasters.Queries.GetTimeLine
{
    public class GetDisasterTimelineQueryHandler
    : IRequestHandler<GetDisasterTimelineQuery, PaginatedList<DisasterTimelineDto>>
    {
        private readonly IDisasterTimeLineRepository _repository;

        public GetDisasterTimelineQueryHandler(IDisasterTimeLineRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<DisasterTimelineDto>> Handle(
            GetDisasterTimelineQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetTimelineAsync(
                request.DisasterId,
                request.Page,
                request.PageSize,
                request.EventType,
                request.Sort,
                cancellationToken);
        }
    }
}

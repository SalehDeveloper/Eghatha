using Eghatha.Domain.Disasters;
using MediatR;

namespace Eghatha.Application.Features.Disasters.Queries.GetDisasterStatuses
{
    public sealed class GetDisasterStatusesQueryHandler : IRequestHandler<GetDisasterStatusesQuery, IReadOnlyList<DisasterStatusResponse>>
    {
        public  Task<IReadOnlyList<DisasterStatusResponse>> Handle(GetDisasterStatusesQuery request, CancellationToken cancellationToken)
        {
            var statuses = DisasterStatus.List.Select(s => new DisasterStatusResponse(s.Value  , s.Name)).ToList();

            return  Task.FromResult<IReadOnlyList<DisasterStatusResponse>>(statuses);
        }
    }
}

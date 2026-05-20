using Eghatha.Domain.Disasters.AffectedPersons;
using MediatR;

namespace Eghatha.Application.Features.Disasters.Queries.GetAffectedPesonHealthStatuses
{
    public sealed class GetHealthStatusesQueryHandler : IRequestHandler<GetHealthStatusesQuery, IReadOnlyList<AffectedPersonHealthStatusResponse>>
    {
        public Task<IReadOnlyList<AffectedPersonHealthStatusResponse>> Handle(GetHealthStatusesQuery request, CancellationToken cancellationToken)
        {
            var statuses = HealthStatus.List.Select(s => new AffectedPersonHealthStatusResponse(s.Value, s.Name)).ToList();

            return Task.FromResult<IReadOnlyList<AffectedPersonHealthStatusResponse>>(statuses);
        }
    }
}

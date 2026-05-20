using Eghatha.Domain.Teams.Resources;
using MediatR;

namespace Eghatha.Application.Features.Teams.Queries.GetResourceStatuses
{
    public sealed class GetResourceStatusesQueryHandler
    : IRequestHandler<GetResourceStatusesQuery, IReadOnlyList<ResourceStatusResponse>>
    {
        public Task<IReadOnlyList<ResourceStatusResponse>> Handle(
            GetResourceStatusesQuery request,
            CancellationToken cancellationToken)
        {
            var result = ResourceStatus.List
                .Select(x => new ResourceStatusResponse(x.Value, x.Name))
                .ToList();

            return Task.FromResult<IReadOnlyList<ResourceStatusResponse>>(result);
        }
    }
}

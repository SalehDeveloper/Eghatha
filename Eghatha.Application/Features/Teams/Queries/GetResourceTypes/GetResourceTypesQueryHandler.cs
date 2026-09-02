using Eghatha.Domain.Teams.TeamResources;
using MediatR;

namespace Eghatha.Application.Features.Teams.Queries.GetResourceTypes
{
    public sealed class GetResourceTypesQueryHandler
    : IRequestHandler<GetResourceTypesQuery, IReadOnlyList<ResourceTypeResponse>>
    {
        public Task<IReadOnlyList<ResourceTypeResponse>> Handle(
            GetResourceTypesQuery request,
            CancellationToken cancellationToken)
        {
            var result = ResourceType.List
                .Select(x => new ResourceTypeResponse(
                    x.Value,
                    x.Name,
                    x.IsConsumable))
                .OrderBy(x=> x.Value)
                .ToList();

            return Task.FromResult<IReadOnlyList<ResourceTypeResponse>>(result);
        }
    }
}

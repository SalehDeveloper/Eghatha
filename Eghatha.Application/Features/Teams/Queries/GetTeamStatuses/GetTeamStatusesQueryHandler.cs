using Eghatha.Domain.Teams;
using MediatR;

namespace Eghatha.Application.Features.Teams.Queries.GetTeamStatuses
{
    public sealed class GetTeamStatusesQueryHandler
    : IRequestHandler<GetTeamStatusesQuery, IReadOnlyList<TeamStatusResponse>>
    {
        public Task<IReadOnlyList<TeamStatusResponse>> Handle(
            GetTeamStatusesQuery request,
            CancellationToken cancellationToken)
        {
            var result = TeamStatus.List
                .Select(x => new TeamStatusResponse(x.Value, x.Name))
                .OrderBy(x=>x.Value)
                .ToList();

            return Task.FromResult<IReadOnlyList<TeamStatusResponse>>(result);
        }
    }
}

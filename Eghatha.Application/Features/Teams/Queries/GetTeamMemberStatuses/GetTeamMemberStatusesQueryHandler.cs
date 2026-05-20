using Eghatha.Domain.Teams.TeamMembers;
using MediatR;

namespace Eghatha.Application.Features.Teams.Queries.GetTeamMemberStatuses
{
    public sealed class GetTeamMemberStatusesQueryHandler
    : IRequestHandler<GetTeamMemberStatusesQuery, IReadOnlyList<TeamMemberStatusResponse>>
    {
        public Task<IReadOnlyList<TeamMemberStatusResponse>> Handle(
            GetTeamMemberStatusesQuery request,
            CancellationToken cancellationToken)
        {
            var result = TeamMemberStatus.List
                .Select(x => new TeamMemberStatusResponse(x.Value, x.Name))
                .ToList();

            return Task.FromResult<IReadOnlyList<TeamMemberStatusResponse>>(result);
        }
    }
}

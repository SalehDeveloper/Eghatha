using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Teams.Dtos;
using MediatR;

namespace Eghatha.Application.Features.Teams.Queries.GetTeamMembers
{
    public class GetTeamMembersQueryHandler
    : IRequestHandler<GetTeamMembersQuery, PaginatedList<TeamMemberDto>>
    {
        private readonly ITeamRepository _repo;

        public GetTeamMembersQueryHandler(ITeamRepository repo)
        {
            _repo = repo;
        }

        public async Task<PaginatedList<TeamMemberDto>> Handle(
            GetTeamMembersQuery request,
            CancellationToken cancellationToken)
        {
            return await _repo.GetTeamMembersAsync(
                request.TeamId,
                request.Page,
                request.PageSize,
                request.SearchTerm,
                request.Status,
                cancellationToken);
        }
    }
}

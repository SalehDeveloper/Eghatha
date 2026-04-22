using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Teams.Dtos;
using MediatR;

namespace Eghatha.Application.Features.Teams.Queries.GetTeamResources
{
    public class GetTeamResourcesQueryHandler
    : IRequestHandler<GetTeamResourcesQuery, PaginatedList<TeamResourceDto>>
    {
        private readonly ITeamRepository _repo;

        public GetTeamResourcesQueryHandler(ITeamRepository repo)
        {
            _repo = repo;
        }

        public async Task<PaginatedList<TeamResourceDto>> Handle(
            GetTeamResourcesQuery request,
            CancellationToken cancellationToken)
        {
            return await _repo.GetTeamResourcesAsync(
                request.TeamId,
                request.Page,
                request.PageSize,
                request.Type,
                cancellationToken);
        }
    }



}

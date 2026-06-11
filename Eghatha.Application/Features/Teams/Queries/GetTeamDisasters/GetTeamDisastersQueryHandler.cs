using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Queries.GetTeamDisasters
{
    public sealed class GetTeamDisastersQueryHandler : IRequestHandler<GetTeamDisastersQuery, PaginatedList<TeamDisastersDto>>
    {
        private readonly ITeamRepository _teamRepository;

        public GetTeamDisastersQueryHandler(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<PaginatedList<TeamDisastersDto>> Handle(GetTeamDisastersQuery request, CancellationToken cancellationToken)
        {
            var res = await _teamRepository.GetTeamDisastersAsync(request.TeamId, request.Page, request.PageSize, cancellationToken);

            return res;
        }
    }
}

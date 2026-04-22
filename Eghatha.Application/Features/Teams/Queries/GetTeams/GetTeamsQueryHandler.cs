using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Teams.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Queries.GetTeams
{
    public class GetTeamsQueryHandler : IRequestHandler<GetTeamsQuery, PaginatedList<TeamDto>>
    {
        private readonly ITeamRepository _teamRepository;

        public GetTeamsQueryHandler(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<PaginatedList<TeamDto>> Handle(GetTeamsQuery request, CancellationToken cancellationToken)
        {
          var teams = await _teamRepository.GetTeamsAsync(
              request.Page,
              request.PageSize,
              request.SearchTerm,
              request.Status,
              request.Speciality,
              request.Province,
              request.City,
              cancellationToken
          );

          return teams;
        }
    }
}

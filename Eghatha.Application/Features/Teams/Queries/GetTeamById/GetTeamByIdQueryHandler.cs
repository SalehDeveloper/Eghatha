using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Features.Teams.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Queries.GetTeamById
{
    public class GetTeamByIdQueryHandler : IRequestHandler<GetTeamByIdQuery, TeamDto>
    {
        private readonly ITeamRepository _teamRepository;

        public GetTeamByIdQueryHandler(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<TeamDto> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
        {
           var team =await _teamRepository.GetTeamOverviewAsync(request.TeamId, cancellationToken);

            return new TeamDto(team.Id ,team.Name, team.Speciality , team.Province , team.City , team.Status , team.LeaderName , team.MembersCount , team.ActiveMembersCount , team.IsReadyForMission);
        }
    }
}
    
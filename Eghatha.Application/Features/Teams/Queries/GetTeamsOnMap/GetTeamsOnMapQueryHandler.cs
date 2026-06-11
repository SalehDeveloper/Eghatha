using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Features.Teams.Dtos;
using MediatR;

namespace Eghatha.Application.Features.Teams.Queries.GetTeamsOnMap
{
    public sealed class GetTeamsOnMapQueryHandler : IRequestHandler<GetTeamsOnMapQuery, List<TeamMapDto>>
    {
        private readonly ITeamRepository _teamRepository;
        public GetTeamsOnMapQueryHandler(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }
        public async Task<List<TeamMapDto>> Handle(GetTeamsOnMapQuery request, CancellationToken cancellationToken)
        {
            var teams = await _teamRepository.GetTeamsOnMapAsync(cancellationToken);

            return teams;
            
        }
    }


}

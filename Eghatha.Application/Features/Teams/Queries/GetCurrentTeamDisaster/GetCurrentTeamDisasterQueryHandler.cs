using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Features.Teams.Queries.GetTeamDisasters;
using ErrorOr;
using MediatR;

namespace Eghatha.Application.Features.Teams.Queries.GetCurrentTeamDisaster
{
    public sealed class GetCurrentTeamDisasterQueryHandler : IRequestHandler<GetCurrentTeamDisasterQuery, ErrorOr<TeamDisastersDto>>
    {
        private readonly ITeamRepository _teamRepository;
        public GetCurrentTeamDisasterQueryHandler(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }
        public async Task<ErrorOr<TeamDisastersDto>> Handle(GetCurrentTeamDisasterQuery request, CancellationToken cancellationToken)
        {
            var teamDisaster = await _teamRepository.GetTeamDisasterAsync(request.TeamId  , cancellationToken);
        
          
            if (teamDisaster is null )
                return ApplicationErrors.NoTeamCurrentDisaster;

            return teamDisaster;
        }
    }



}

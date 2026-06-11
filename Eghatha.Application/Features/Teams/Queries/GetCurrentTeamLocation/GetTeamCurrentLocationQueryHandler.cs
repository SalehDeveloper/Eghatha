using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Services;
using ErrorOr;
using MediatR;

namespace Eghatha.Application.Features.Teams.Queries.GetCurrentTeamLocation
{
    public sealed class GetTeamCurrentLocationQueryHandler : IRequestHandler<GetTeamCurrentLocationQuery, ErrorOr<TeamLocation>>
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ITeamOperationalLocationProvider _teamOperationalLocationProvider;

        public GetTeamCurrentLocationQueryHandler(ITeamRepository teamRepository, ITeamOperationalLocationProvider teamOperationalLocationProvider)
        {
            _teamRepository = teamRepository;
            _teamOperationalLocationProvider = teamOperationalLocationProvider;
        }

        public async Task<ErrorOr<TeamLocation>> Handle(GetTeamCurrentLocationQuery request, CancellationToken cancellationToken)
        {
            var team = await _teamRepository.GetByIdAsync(request.TeamId, cancellationToken);

            if (team is null) return ApplicationErrors.TeamNotFound;

            var teamLocationResult = await _teamOperationalLocationProvider.GetLocationAsync(team, cancellationToken);

            return new TeamLocation(teamLocationResult.location.Latitude , teamLocationResult.location.Longitude, teamLocationResult.isLiveLocation);
        }
    }
}

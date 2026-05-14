using Eghatha.Application.Common.Services;
using Eghatha.Domain.Shared.ValueObjects;
using Eghatha.Domain.Teams;

namespace Eghatha.Infastructure.Services
{
    public sealed class TeamOperationalLocationProvider
    : ITeamOperationalLocationProvider
    {
        private readonly ITeamLocationService _teamLocationService;

        public TeamOperationalLocationProvider(
            ITeamLocationService teamLocationService)
        {
            _teamLocationService = teamLocationService;
        }

        public async Task<(GeoLocation location, bool isLiveLocation)> GetLocationAsync(
            Team team,
            CancellationToken cancellationToken)
        {
            var liveLocation = await _teamLocationService.GetLocationAsync(team.Id);

            if (liveLocation is not null)
                return (liveLocation, true);

            return (team.Location, false);
        }
    }
}

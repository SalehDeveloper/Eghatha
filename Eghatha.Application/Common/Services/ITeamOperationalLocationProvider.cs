using Eghatha.Domain.Shared.ValueObjects;
using Eghatha.Domain.Teams;

namespace Eghatha.Application.Common.Services
{
    public interface ITeamOperationalLocationProvider
    {
        Task<(GeoLocation location, bool isLiveLocation)> GetLocationAsync(
            Team team,
            CancellationToken cancellationToken);
    }
}

using Eghatha.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Services
{
    public interface ITeamLocationService
    {
        Task SetLocationAsync(Guid teamId, GeoLocation location);
        Task<GeoLocation?> GetLocationAsync(Guid teamId);
        Task<IReadOnlyList<(Guid teamId, double distance)>> GetNearbyTeamsAsync(
            GeoLocation location, double radiusKm);
    }
}

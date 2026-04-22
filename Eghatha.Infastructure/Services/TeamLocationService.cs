using Eghatha.Application.Common.Services;
using Eghatha.Domain.Shared.ValueObjects;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Services
{
    public class TeamLocationService : ITeamLocationService
    {
        private const string Key = "teams:geo";
        private readonly IDatabase _db;

        public TeamLocationService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task SetLocationAsync(Guid teamId, GeoLocation location)
        {
            await _db.GeoAddAsync(
                Key,
                location.Longitude,
                location.Latitude,
                teamId.ToString());
        }

        public async Task<GeoLocation?> GetLocationAsync(Guid teamId)
        {
            var result = await _db.GeoPositionAsync(Key, teamId.ToString());

            var pos = result;
            if (pos == null) return null;

            return GeoLocation.Create(pos.Value.Latitude, pos.Value.Longitude).Value;
        }

        public async Task<IReadOnlyList<(Guid teamId, double distance)>> GetNearbyTeamsAsync(
            GeoLocation location,
            double radiusKm)
        {
            var results = await _db.GeoRadiusAsync(
                Key,
                location.Longitude,
                location.Latitude,
                radiusKm,
                GeoUnit.Kilometers,
                count: 50,
                order: Order.Ascending);

            return results
                .Select(r => (Guid.Parse(r.Member!), r.Distance ?? 0))
                .ToList();
        }
    }
}

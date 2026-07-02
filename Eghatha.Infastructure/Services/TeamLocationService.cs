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
        private static string LiveKey(Guid teamId) => $"team:location:live:{teamId}";
        private static readonly TimeSpan LiveLocationTtl = TimeSpan.FromMinutes(2);
        private readonly IDatabase _db;

        public TeamLocationService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }


        public async Task SetLocationAsync(Guid teamId, GeoLocation location)
        {
            var payload = $"{location.Latitude}|{location.Longitude}";
            await _db.StringSetAsync(LiveKey(teamId), payload, LiveLocationTtl);
        }

        public async Task<GeoLocation?> GetLocationAsync(Guid teamId)
        {
            var value = await _db.StringGetAsync(LiveKey(teamId));
            if (value.IsNullOrEmpty) return null;

            var parts = value.ToString().Split('|');
            if (parts.Length != 2) return null;

            return GeoLocation.Create(double.Parse(parts[0]), double.Parse(parts[1])).Value;
        }

        public async Task RemoveLocationAsync(Guid teamId)
        {
            await _db.KeyDeleteAsync(LiveKey(teamId));
        }
    }
}

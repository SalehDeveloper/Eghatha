using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _db;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisCacheService(IConfiguration config)
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect(config["Redis:Connection"]);
            _db = _connectionMultiplexer.GetDatabase();
        }

        public async Task SetAsync(string key, string value, TimeSpan? expiry = null)
        {
            if (expiry.HasValue)
                await _db.StringSetAsync(key, value, expiry.Value);
            else
                await _db.StringSetAsync(key, value);
        }

        public async Task<string?> GetAsync(string key)
        {
            var value = await _db.StringGetAsync(key);
            return value.IsNull ? null : value.ToString();
        }

        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }
    }

  
}
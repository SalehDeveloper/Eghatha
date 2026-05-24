using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Services;
using Microsoft.Extensions.Caching.Hybrid;

namespace Eghatha.Infastructure.Services
{
    public class IdentityCacheService : IIdentityCacheService
    {
        private const string AdminKey = "identity:admins";

        private readonly HybridCache _cache;
        private readonly IIdentityService _identityService;

        public IdentityCacheService(
            HybridCache cache,
            IIdentityService identityService)
        {
            _cache = cache;
            _identityService = identityService;
        }

        public async Task<List<Guid>> GetAdminIdsAsync(CancellationToken cancellationToken)
        {
            return await _cache.GetOrCreateAsync(
                AdminKey,
                async _ => await _identityService.GetAdminIdsAsync(cancellationToken),
                new HybridCacheEntryOptions
                {
                    Expiration = TimeSpan.FromMinutes(60)
                },
                cancellationToken: cancellationToken);
        }

        public async Task InvalidateAdminsAsync()
        {
            await _cache.RemoveAsync(AdminKey);
        }
    }
}

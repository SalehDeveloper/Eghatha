using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;



public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly HybridCache _cache;
    private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

    public CachingBehavior(HybridCache cache, ILogger<CachingBehavior<TRequest, TResponse>> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not ICachedQuery cachedRequest)
            return await next(cancellationToken);

        var cacheKey = cachedRequest.CachKey;

        var options = new HybridCacheEntryOptions
        {
            Expiration = cachedRequest.Expiration
        };

        try
        {
            var response = await _cache.GetOrCreateAsync<TResponse>(
                cacheKey,
                async ct =>
                {
                    _logger.LogDebug("Cache MISS for key: {CacheKey}", cacheKey);

                    var result = await next(ct);

                    return result;
                },
                options,
                cachedRequest.Tags,
                cancellationToken
            );

            _logger.LogDebug("Cache HIT for key: {CacheKey}", cacheKey);

            if (response is IErrorOr errorOr && errorOr.IsError)
            {
                _logger.LogWarning("Cached response contains error. Removing cache key: {CacheKey}", cacheKey);

                await _cache.RemoveAsync(cacheKey, cancellationToken);
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Caching failed for key: {CacheKey}", cacheKey);

            
            return await next(cancellationToken);
        }
    }
}



using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;



public class CachingBehavior<TRequest , TResponse> (
    HybridCache cache , 
    ILogger<CachingBehavior<TRequest , TResponse>> logger
): IPipelineBehavior<TRequest , TResponse>
where TRequest : notnull
{
    private readonly HybridCache _cache = cache;

    private readonly ILogger<CachingBehavior<TRequest , TResponse>> _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not ICachedQuery cachedRequest)
         return await next(cancellationToken);

        _logger.LogInformation("Checking cache for {RequestName}", typeof(TRequest).Name);

        var result = await _cache.GetOrCreateAsync<TResponse>(
            cachedRequest.CachKey , 
            _ => new ValueTask<TResponse>((TResponse)(object)null!),
            new HybridCacheEntryOptions
            {
                Flags = HybridCacheEntryFlags.DisableUnderlyingData
            } , cancellationToken:cancellationToken
            );

            if (result is null )
        {
            result = await next(cancellationToken);

            if (result is IErrorOr res && res.IsError == false)
            {
                 _logger.LogInformation("Caching result for {RequestName}", typeof(TRequest).Name);
                  
                await _cache.SetAsync(
                    cachedRequest.CachKey,
                    result,
                    new HybridCacheEntryOptions
                    {
                         Expiration = cachedRequest.Expiration
                    },
                    cachedRequest.Tags,
                    cancellationToken
                );
            }
        }
        
            return result;
    
    }
}
using MediatR;
using Microsoft.Extensions.Logging;
public class UnhandledExceptionBehavior<TRequest , TResponse>
(ILogger<TRequest> logger): IPipelineBehavior<TRequest , TResponse>
where TRequest : notnull
{
    

    private  readonly ILogger<TRequest> _logger = logger;

    public async Task<TResponse> Handle(TRequest request , RequestHandlerDelegate<TResponse> next , CancellationToken token)
    {
        try
        {
            return await next(token);
        }catch(Exception ex)
        {
            var requestName = typeof(TRequest).Name;

             _logger.LogError(ex, "Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

             throw;
        }
    }
}
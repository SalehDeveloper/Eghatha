using MediatR;

public interface ICachedQuery
{
    string CachKey{get;}
    string[] Tags{get;}
    TimeSpan Expiration{get;}
}

public interface ICachedQuery<TResponse>:IRequest<TResponse> , ICachedQuery;
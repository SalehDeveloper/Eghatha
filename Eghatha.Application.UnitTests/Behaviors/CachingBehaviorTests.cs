using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using NSubstitute;
using static Google.Apis.Requests.BatchRequest;


namespace Eghatha.Application.UnitTests.Behaviors
{
    public class CachingBehaviorTests
    {
        private readonly HybridCache _cache = Substitute.For<HybridCache>();

        private readonly ILogger<CachingBehavior<CachedQuery, string>> _logger = Substitute.For<ILogger<CachingBehavior<CachedQuery, string>>>();

        private readonly CachingBehavior<CachedQuery, string> _sut;

        public CachingBehaviorTests()
        {
            _sut = new CachingBehavior<CachedQuery, string>(_cache, _logger);
        }




        [Fact]
        public  async Task Handle_WhenRequestIsNotCachedQuery_ShouldSkipCahceAndReturnResult()
        {
            // Arrange
            var UnChachedRequest = new NonCachedQuery();
            var behavior = new CachingBehavior<NonCachedQuery, string>(_cache, Substitute.For<ILogger<CachingBehavior<NonCachedQuery, string>>>());  
            
            // Act
            var result = await behavior.Handle(UnChachedRequest, _ => Task.FromResult("Not Cached"), CancellationToken.None);
            
           
            // Assert
            Assert.Equal("Not Cached", result);
            await _cache.DidNotReceive().SetAsync(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<HybridCacheEntryOptions>(),
            Arg.Any<string[]>(),
            Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_WhenRequestIsCachedQuery_ShouldCahceResult()
        {
            // Arrange
            var cachedRequest = new CachedQuery();
            var expectedResult = "Cached Result";
            string? actualKey = null;
            object? actualValue = null;
            HybridCacheEntryOptions? actualOptions = null;
            string[]? actualTags = null;
            CancellationToken actualToken = default;
            // Act
            _cache.SetAsync(
            Arg.Do<string>(k => actualKey = k),
            Arg.Do<object>(v => actualValue = v),
            Arg.Do<HybridCacheEntryOptions>(o => actualOptions = o),
            Arg.Do<string[]>(t => actualTags = t),
            Arg.Do<CancellationToken>(c => actualToken = c)).Returns(ValueTask.CompletedTask);

            // Act
            var result = await _sut.Handle(cachedRequest, _ => Task.FromResult(expectedResult), CancellationToken.None);

            // Assert
        
            Assert.Equal(cachedRequest.CachKey, actualKey);

            Assert.Equal(cachedRequest.Expiration, actualOptions!.Expiration);
            Assert.Equal(cachedRequest.Tags, actualTags);
            Assert.Equal(expectedResult, actualValue);



        }

        [Fact]
        public async Task Handle_WhenResultIsCached_ShouldReturnCachedResultWithoutCallingHandler()
        {
            // Arrange
            var request = new CachedQuery();
            var cachedResponse = "cached-value";

            _cache.GetOrCreateAsync<string>(
         request.CachKey,
         Arg.Any<Func<CancellationToken, ValueTask<string>>>(),
         Arg.Any<HybridCacheEntryOptions>(),
         Arg.Any<IEnumerable<string>>(),
         Arg.Any<CancellationToken>())
         .Returns(cachedResponse);

            var handlerCalled = false;

            // Act
            var result = await _sut.Handle(
                request,
                _ =>
                {
                    handlerCalled = true;
                    return Task.FromResult("handler-value");
                },
                CancellationToken.None);

            // Assert
         
            Assert.Equal("cached-value", result);
            Assert.False(handlerCalled);
        }

    }

    public class NonCachedQuery;

    public class CachedQuery : ICachedQuery
    {
  
        public TimeSpan Expiration => TimeSpan.FromMinutes(5);
        public string[] Tags => ["unit-test"];

        public string CachKey => "test-key";
    }
}

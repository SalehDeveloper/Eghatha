using Castle.Core.Logging;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.UnitTests.Behaviors
{
    public class UnhandledExceptionBehaviorTests
    {
        private readonly ILogger<TestRequest> _logger = Substitute.For<ILogger<TestRequest>>();

        private readonly UnhandledExceptionBehavior<TestRequest , string> _sut;

        public UnhandledExceptionBehaviorTests()
        {
            _sut = new UnhandledExceptionBehavior<TestRequest, string> (_logger);
        }


        [Fact]
        public async Task Handle_WhenNoException_InvokesNextAndReturnsResult()
        {
            // Arrange
            var request = new TestRequest();
            var next = Substitute.For<RequestHandlerDelegate<string>>();
            next.Invoke().Returns("OK");

            // Act
            var result = await _sut.Handle(request, next, CancellationToken.None);

            // Assert
            Assert.Equal("OK", result);
        }

        [Fact]
        public async Task Handle_WhenExceptionThrown_LogsErrorAndRethrows()
        {
            // Arrange
            var request = new TestRequest();
            var exception = new InvalidOperationException("test failure");

            var next = Substitute.For<RequestHandlerDelegate<string>>();
            next.Invoke().Returns<Task<string>>(_ => throw exception);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _sut.Handle(request, next, CancellationToken.None));

            Assert.Equal(exception, ex);

            _logger.Received(1).Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString()!.Contains("Unhandled Exception")),
                exception,
                Arg.Any<Func<object, Exception?, string>>());
        }
    }

    public class TestRequest;
}

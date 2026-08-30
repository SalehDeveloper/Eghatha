using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Behaviors;
using Eghatha.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.UnitTests.Behaviors
{
    public class LoggingBehaviorTests
    {
        private readonly ILogger<TestRequest> _logger = Substitute.For<ILogger<TestRequest>>();
        private readonly IUser _user = Substitute.For<IUser>();
        private readonly IIdentityService _identityService = Substitute.For<IIdentityService>();

        private readonly LoggingBehavior<TestRequest> _sut;

        public LoggingBehaviorTests()
        {
            _sut = new LoggingBehavior<TestRequest>(_logger, _user, _identityService);
        }

        [Fact]
        public async Task Process_WithUserId_LogsRequestWithUserName()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new TestRequest();

            _user.Id.Returns(id);
            _identityService
                .GetUserNameAsync(id)
                .Returns("Issam");

            // Act
            await _sut.Process(request, CancellationToken.None);

            // Assert
            await _identityService
                .Received(1)
                .GetUserNameAsync(id);

            _logger.ReceivedWithAnyArgs(1).Log(
                default!,
                default!,
                default!,
                default!,
                default!);
        }

        [Fact]
        public async Task Process_WithoutUserId_LogsRequestWithEmptyUserName()
        {
            // Arrange
            var request = new TestRequest();

            _user.Id.Returns(Guid.Empty);

            // Act
            await _sut.Process(request, CancellationToken.None);

            // Assert
            await _identityService
                .DidNotReceive()
                .GetUserNameAsync(Arg.Any<Guid>());

            _logger.ReceivedWithAnyArgs(1).Log(
                default!,
                default!,
                default!,
                default!,
                default!);
        }

        public class TestRequest;
    }
}

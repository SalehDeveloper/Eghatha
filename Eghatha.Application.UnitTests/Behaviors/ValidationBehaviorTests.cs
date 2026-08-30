using Eghatha.Application.Features.Teams.Commands.DecreaseTeamReosurce;
using ErrorOr;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.UnitTests.Behaviors
{
    public class ValidationBehaviorTests
    {
        private readonly ValidationBehavior<
       DecreaseTeamResourceCommand,
       ErrorOr<Updated>> _sut;

        private readonly IValidator<DecreaseTeamResourceCommand> _mockValidator;

        private readonly RequestHandlerDelegate<ErrorOr<Updated>> _mockNextBehavior;

        public ValidationBehaviorTests()
        {
            _mockValidator =
                Substitute.For<IValidator<DecreaseTeamResourceCommand>>();

            _mockNextBehavior =
                Substitute.For<RequestHandlerDelegate<ErrorOr<Updated>>>();

            _sut = new ValidationBehavior<
                DecreaseTeamResourceCommand,
                ErrorOr<Updated>>(_mockValidator);
        }

        [Fact]
        public async Task Handle_WhenValidationIsValid_ShouldInvokeNextBehavior()
        {
            // Arrange
            var command = new DecreaseTeamResourceCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                5);

            var expectedResponse = Result.Updated;

            _mockValidator
                .ValidateAsync(
                    command,
                    Arg.Any<CancellationToken>())
                .Returns(new ValidationResult());

            _mockNextBehavior
                .Invoke()
                .Returns(expectedResponse);

            // Act
            var result = await _sut.Handle(
                command,
                _mockNextBehavior,
                CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.True(!result.IsError);
            Assert.Equal(expectedResponse, result);

            await _mockNextBehavior.Received(1).Invoke();
        }

        [Fact]
        public async Task Handle_WhenValidationIsInvalid_ShouldReturnValidationErrors()
        {
            // Arrange
            var command = new DecreaseTeamResourceCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                0);

            var validationFailures = new List<ValidationFailure>
    {
        new(
            propertyName: nameof(DecreaseTeamResourceCommand.Quantity),
            errorMessage: "Quantity must be greater than zero")
    };

            _mockValidator
                .ValidateAsync(
                    command,
                    Arg.Any<CancellationToken>())
                .Returns(new ValidationResult(validationFailures));

            // Act
            var result = await _sut.Handle(
                command,
                _mockNextBehavior,
                CancellationToken.None);

            // Assert
            Assert.True(result.IsError);

            Assert.Equal(
                nameof(DecreaseTeamResourceCommand.Quantity),
                result.FirstError.Code);

            Assert.Equal(
                "Quantity must be greater than zero",
                result.FirstError.Description);

            await _mockNextBehavior
                .DidNotReceive()
                .Invoke();
        }

        [Fact]
        public async Task Handle_WhenNoValidator_ShouldInvokeNextBehavior()
        {
            // Arrange
            var command = new DecreaseTeamResourceCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                5);

            var behavior =
                new ValidationBehavior<
                    DecreaseTeamResourceCommand,
                    ErrorOr<Updated>>();

            var expectedResponse = Result.Updated;

            _mockNextBehavior
                .Invoke()
                .Returns(expectedResponse);

            // Act
            var result = await behavior.Handle(
                command,
                _mockNextBehavior,
                CancellationToken.None);

            // Assert
            Assert.True(!result.IsError);
            Assert.Equal(expectedResponse, result);

            await _mockNextBehavior
                .Received(1)
                .Invoke();
        }
    }
}

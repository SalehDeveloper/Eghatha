using Eghatha.Domain.Identity;
using Eghatha.Domain.Shared.Errors;
using Eghatha.Tests.Common.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.UnitTests.Identity
{
    public class RefreshTokenTests
    {
        // ---------- Create ----------

        [Fact]
        public void Create_WithValidData_ReturnsUnrevokedRefreshTokenWithExpectedValues()
        {
            var userId = Guid.NewGuid();
            var expiresOnUtc = DateTimeOffset.UtcNow.AddDays(7);

            var result = RefreshTokenBuilder.Valid()
                .WithUserId(userId)
                .WithToken("a-valid-token")
                .WithExpiresOnUtc(expiresOnUtc)
                .Build();

            Assert.False(result.IsError);
            var refreshToken = result.Value;
            Assert.NotEqual(Guid.Empty, refreshToken.Id);
            Assert.Equal(userId, refreshToken.UserId);
            Assert.Equal("a-valid-token", refreshToken.Token);
            Assert.Equal(expiresOnUtc, refreshToken.ExpiresOnUtc);
            Assert.False(refreshToken.IsRevoked);
        }

        [Fact]
        public void Create_WithEmptyUserId_ReturnsIdMustBeProvidedError()
        {
            var result = RefreshTokenBuilder.Valid().WithUserId(Guid.Empty).Build();

            Assert.True(result.IsError);
            Assert.Equal(DomainErrors.IdMustBeProvided("User"), result.FirstError);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithMissingToken_ReturnsTokenRequiredError(string? token)
        {
            var result = RefreshTokenBuilder.Valid().WithToken(token!).Build();

            Assert.True(result.IsError);
            Assert.Equal(RefreshTokenErrors.TokenRequired, result.FirstError);
        }

        [Fact]
        public void Create_WithExpiryInThePast_ReturnsExpiryInvalidError()
        {
            var result = RefreshTokenBuilder.Valid()
                .WithExpiresOnUtc(DateTimeOffset.UtcNow.AddMinutes(-1))
                .Build();

            Assert.True(result.IsError);
            Assert.Equal(RefreshTokenErrors.ExpiryInvalid, result.FirstError);
        }

        [Fact]
        public void Create_WithExpiryInTheFuture_Succeeds()
        {
            var result = RefreshTokenBuilder.Valid()
                .WithExpiresOnUtc(DateTimeOffset.UtcNow.AddMinutes(1))
                .Build();

            Assert.False(result.IsError);
        }

        // ---------- Revoke ----------

        [Fact]
        public void Revoke_WhenNotRevoked_SetsIsRevokedTrue()
        {
            var refreshToken = RefreshTokenTestFactory.CreateValid();

            var result = refreshToken.Revoke();

            Assert.False(result.IsError);
            Assert.True(refreshToken.IsRevoked);
        }

        [Fact]
        public void Revoke_WhenAlreadyRevoked_ReturnsTokenAlreadyRevokedError()
        {
            var refreshToken = RefreshTokenTestFactory.CreateRevoked();

            var result = refreshToken.Revoke();

            Assert.True(result.IsError);
            Assert.Equal(RefreshTokenErrors.TokenAlreadyRevoked, result.FirstError);
        }
    }
}

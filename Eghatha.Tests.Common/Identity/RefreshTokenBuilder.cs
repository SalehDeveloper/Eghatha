using Eghatha.Domain.Identity;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Tests.Common.Identity
{
    /// <summary>
    /// Fluent builder that produces a valid, non-expired <see cref="RefreshToken"/>
    /// by default. Use the With* methods to override individual fields when a
    /// test needs to exercise a specific validation branch (e.g. an expiry in
    /// the past).
    ///
    /// NOTE: RefreshToken.Create generates its own Id internally (it isn't
    /// accepted as a parameter), so there's no WithId here.
    /// </summary>
    public sealed class RefreshTokenBuilder
    {
        private Guid _userId = Guid.NewGuid();
        private string _token = "test-refresh-token";
        private DateTimeOffset _expiresOnUtc = DateTimeOffset.UtcNow.AddDays(7);

        public static RefreshTokenBuilder Valid() => new();

        public RefreshTokenBuilder WithUserId(Guid userId)
        {
            _userId = userId;
            return this;
        }

        public RefreshTokenBuilder WithToken(string token)
        {
            _token = token;
            return this;
        }

        public RefreshTokenBuilder WithExpiresOnUtc(DateTimeOffset expiresOnUtc)
        {
            _expiresOnUtc = expiresOnUtc;
            return this;
        }

        public ErrorOr<RefreshToken> Build() =>
            RefreshToken.Create(_userId, _token, _expiresOnUtc);

        /// <summary>
        /// Builds and unwraps the result. Only use this in tests where the
        /// input is known-valid (arrange phase) — never in the tests that
        /// are actually asserting on Create's validation behavior.
        /// </summary>
        public RefreshToken BuildValid() => Build().Value;
    }
}

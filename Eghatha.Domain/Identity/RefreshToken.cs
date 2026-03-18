using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Shared.Errors;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Identity
{
    public sealed class RefreshToken: AuditableEntity
    {
        public Guid UserId { get; private set; }

        public string Token { get; private set; }

        public DateTimeOffset ExpiresOnUtc { get; private set; }

        public bool IsRevoked { get; private set; }

        private RefreshToken(Guid id , Guid userId , string token  , DateTimeOffset expiresOnUtc)
            :base(id)
        {
            UserId = userId;
            Token = token;
            ExpiresOnUtc = expiresOnUtc;
            IsRevoked = false;
        }

        public static ErrorOr<RefreshToken> Create (Guid userId , string token , DateTimeOffset expiresOnUtc)
        {
            if (userId == Guid.Empty)
            {
                return DomainErrors.IdMustBeProvided("User");
            }
            if (string.IsNullOrWhiteSpace(token))
            {
                return RefreshTokenErrors.TokenRequired;
            }
            if (expiresOnUtc <= DateTimeOffset.UtcNow)
            {
                return RefreshTokenErrors.ExpiryInvalid;
            }
            var refreshToken = new RefreshToken(Guid.NewGuid(), userId, token, expiresOnUtc);
           
            return refreshToken;
        }

        public ErrorOr<Updated> Revoke()
        {
             if (IsRevoked)
            {
                return RefreshTokenErrors.TokenAlreadyRevoked;
            }

            IsRevoked = true;
            return Result.Updated;
        }
    }
}

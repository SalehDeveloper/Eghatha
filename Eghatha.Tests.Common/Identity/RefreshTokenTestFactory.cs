using Eghatha.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Tests.Common.Identity
{
    /// <summary>
    /// Produces RefreshToken aggregates already advanced to a given lifecycle
    /// state. Every transition goes through the aggregate's own public
    /// methods (never reflection/internal state hacks).
    /// </summary>
    public static class RefreshTokenTestFactory
    {
        public static RefreshToken CreateValid() => RefreshTokenBuilder.Valid().BuildValid();

        public static RefreshToken CreateRevoked()
        {
            var token = CreateValid();
            token.Revoke();
            return token;
        }
    }
}

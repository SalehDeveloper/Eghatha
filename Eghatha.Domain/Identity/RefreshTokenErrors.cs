using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Identity
{
    public static class RefreshTokenErrors
    {
        public static readonly Error TokenRequired =
         Error.Validation(
             code:"RefreshToken_Token_Required",
             description:"Token value is required.");

        public static readonly Error ExpiryInvalid =
        Error.Validation(
            code:"RefreshToken_Expiry_Invalid",
            description:"Expiry must be in the future.");

        public static readonly Error TokenAlreadyRevoked =Error.Conflict(
            code:"RefreshToken_Already_Revoked",
            description:"Token has already been revoked.");
    }
}

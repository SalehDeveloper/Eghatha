using ErrorOr;

namespace Eghatha.Domain.Identity
{
    public static class RefreshTokenErrors
    {
        public static readonly Error TokenRequired =
         Error.Validation(
             code: "RefreshToken_Token_Required",
             description: Resources.RefreshTokenErrors.RefreshToken_Token_Required);

        public static readonly Error ExpiryInvalid =
        Error.Validation(
            code: "RefreshToken_Expiry_Invalid",
            description: Resources.RefreshTokenErrors.RefreshToken_Expiry_Invalid);

        public static readonly Error TokenAlreadyRevoked = Error.Conflict(
            code: "RefreshToken_Already_Revoked",
            description: Resources.RefreshTokenErrors.RefreshToken_Already_Revoked);
    }
}
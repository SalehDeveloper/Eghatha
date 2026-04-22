using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Errors
{
    public  static class ApplicationErrors
    {
        public static readonly Error InvalidRefreshToken = Error.Unauthorized(
           code: "Auth.InvalidRefreshToken",
           description: "Invalid RefreshToken.");

        public static readonly Error InvalidOtp = Error.Conflict(
           code: "Auth.InvalidOtp",
           description: "Otp code is wrong or expired.");


        public static readonly Error EmailAlreadyConfirmed = Error.Conflict(
           code: "Auth.EmailAlreadyConfirmed",
           description: "Email is already confirmed.");

        public static readonly Error TeamNotFound = Error.NotFound(
            code: "Team.NotFound",
            description: "Team not found.");
    }
}

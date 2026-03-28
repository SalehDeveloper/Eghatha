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
    }
}

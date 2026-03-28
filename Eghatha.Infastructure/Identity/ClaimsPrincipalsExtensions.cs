using Eghatha.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Identity
{
    public static class ClaimsPrincipalsExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal? claims)
        {
            var userId = claims?.FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(userId, out Guid parsedUserId)
                ? parsedUserId
                : throw new ApplicationException("User id is unavailable");
        }

        public static string GetUserRole(this ClaimsPrincipal claims)
        {
            var role = claims.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrWhiteSpace(role))
                throw new UnauthorizedAccessException("Role not found");

            return role;
        }
    }
}

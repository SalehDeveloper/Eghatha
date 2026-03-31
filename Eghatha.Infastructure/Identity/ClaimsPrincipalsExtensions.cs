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
        public static Guid? GetUserId(this ClaimsPrincipal? claims)
        {
            var userId = claims?.FindFirstValue("userId");

            return Guid.TryParse(userId, out Guid parsedUserId)
                ? parsedUserId
                : null;
        }
        

        public static List<string> GetUserRole(this ClaimsPrincipal claims)
        {
            var roles = claims.FindAll(ClaimTypes.Role)
                        .Select(c => c.Value)
                        .Where(r => !string.IsNullOrWhiteSpace(r))
                        .ToList();

            if (roles.Count == 0)
                throw new UnauthorizedAccessException("Roles not found");

            return roles;
        }
    }
}

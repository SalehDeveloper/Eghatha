using Eghatha.Domain.Identity;
using System.Security.Claims;

namespace Eghatha.Application.Common.Authentication
{
    public sealed record AppUserDto(Guid UserId, string Email, IList<string> Roles, IList<Claim> Claims);
}

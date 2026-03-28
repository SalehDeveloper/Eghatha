using Eghatha.Domain.Identity;

namespace Eghatha.Application.Common.Authentication
{
    public sealed record AppUserDto(Guid UserId, string Email, IList<string> Roles);
}

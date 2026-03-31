using Eghatha.Application.Common.Models;
using Eghatha.Contract.Accounts.Responses;

namespace Eghatha.Api.Mappers
{
    public static class AccountMapper
    {

        public static AccountResponse MapToAccount (this IdentityUser user)
        {
            return new AccountResponse(user.Id, user.Email, user.Roles, user.PhoneNumber, user.FirstName, user.LastName, user.PhotoUrl, user.IsActive);
        }

        public static IReadOnlyCollection<AccountResponse> MapToAccounts(this IReadOnlyCollection<IdentityUser> users)
        {
            return users.Select(u =>u.MapToAccount()).ToList();
        }
    }
}

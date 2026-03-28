using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Authentication
{
    public interface IIdentityService
    {
        Task<ErrorOr< AppUserDto>> AuthenticateAsync(string email, string password , CancellationToken cancellationToken);

        Task<ErrorOr<AppUserDto>> GetUserByIdAsync (Guid userId, CancellationToken cancellationToken);
    }
}

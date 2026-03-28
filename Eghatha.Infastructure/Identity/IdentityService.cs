using Eghatha.Application.Common.Authentication;
using Eghatha.Domain.Identity;
using Eghatha.Infastructure.Data;
using ErrorOr;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ErrorOr<AppUserDto>> AuthenticateAsync(string email, string password , CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return Error.NotFound("User_Not_Found", $"User with email {(email)} not found");

            if (!user.EmailConfirmed)
                return Error.Conflict("Email_Not_Confirmed", $"email '{(email)}' not confirmed");

            if (!user.IsActive)
                return Error.Conflict("User_Not_Actuve", $"user with '{(email)}' not active");

            if ( await _userManager.CheckPasswordAsync(user , password) is false)
                return Error.Conflict("Invalid_Login_Attempt", "Email / Password are incorrect");

            return new AppUserDto(user.Id , user.Email , await _userManager.GetRolesAsync(user));





        }

        public async Task<ErrorOr<AppUserDto>> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user =await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return Error.NotFound("User_Not_Found", $"User with id {(userId)} not found");
            
            var roles = await _userManager.GetRolesAsync(user);

            return new AppUserDto(userId , user.Email , roles);
        }
    }
}

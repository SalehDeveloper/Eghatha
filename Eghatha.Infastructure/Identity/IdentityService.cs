using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Authentication.Dtos;
using Eghatha.Domain.Identity;
using Eghatha.Infastructure.Data;
using ErrorOr;
using MailKit.Search;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityUser = Eghatha.Application.Common.Models.IdentityUser;

namespace Eghatha.Infastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _appDbContext;

        public IdentityService(UserManager<ApplicationUser> userManager, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }


        public async Task<ErrorOr<Success>> AddUserToRoleAsync (Guid userId ,  Role role)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var res = await _userManager.AddToRoleAsync( user , role.ToString());

            return Result.Success;
        }
        public async Task<ErrorOr<Guid>> CreatUserAsync (string firstName , string lastName , string email ,string phoneNumber ,  string? password ,string photoUrl, UserCreationMode mode)
        {
           

          
            var user = new ApplicationUser(firstName, lastName, email, phoneNumber, photoUrl);

            IdentityResult result;

         
            switch (mode)
            {
                case UserCreationMode.Regular:
                    {
                        if (string.IsNullOrWhiteSpace(password))
                            return Error.Validation("Password.Required", "Password is required for regular users");

                        result = await _userManager.CreateAsync(user, password);
                     
                        break;
                    }

                case UserCreationMode.Invited:
                    {
                        
                        result = await _userManager.CreateAsync(user);
                        user.EmailConfirmed = true;
                        user.Activate();
                       break;
                    }

                default:
                    return Error.Validation("Invalid_Mode", "Invalid user creation mode");
            }

         
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e =>
                    Error.Failure(e.Code, e.Description));

                return errors.ToArray();
            }

            
            return user.Id;

        }
        public async Task<ErrorOr<AppUserDto>> AuthenticateAsync(string email, string password, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return Error.NotFound("User_Not_Found", $"User with email {(email)} not found");

            if (!user.EmailConfirmed)
                return Error.Conflict("Email_Not_Confirmed", $"email '{(email)}' not confirmed");

            if (!user.IsActive)
                return Error.Conflict("User_Not_Actuve", $"user with '{(email)}' not active");

            if (await _userManager.CheckPasswordAsync(user, password) is false)
                return Error.Conflict("Invalid_Login_Attempt", "Email / Password are incorrect");

            return new AppUserDto(user.Id, user.Email, await _userManager.GetRolesAsync(user));





        }

        public async Task<ErrorOr<Application.Common.Models.IdentityUser>> GetIdentityUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return Error.NotFound("User_Not_Found", $"User with email {(email)} not found");

            var roles = await _userManager.GetRolesAsync(user);

            return new Application.Common.Models.IdentityUser(user.Id, user.Email, roles, user.PhoneNumber, user.FirstName, user.LastName, user.PhotoUrl, user.IsActive, user.EmailConfirmed);
        }

        public async Task<ErrorOr<AppUserDto>> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return Error.NotFound("User_Not_Found", $"User with id {(userId)} not found");

            var roles = await _userManager.GetRolesAsync(user);

            return new AppUserDto(userId, user.Email, roles);
        }

        public async Task<ErrorOr<Application.Common.Models.IdentityUser>> GetUserDetailsByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null) return Error.NotFound("User_Not_Found", $"User with id {(userId)} not found");

            var roles = await _userManager.GetRolesAsync(user);

            return new Application.Common.Models.IdentityUser(user.Id, user.Email, roles, user.PhoneNumber, user.FirstName, user.LastName, user.PhotoUrl, user.IsActive, user.EmailConfirmed);
        }

        public async Task<ErrorOr<Success>> ResetPasswordAsync(string email, string newPassword, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return Error.NotFound("User_Not_Found", $"User with email {(email)} not found");

            // Remove old password (safe approach)
            var removeResult = await _userManager.RemovePasswordAsync(user);

            if (!removeResult.Succeeded)
                return Error.Failure("password reset failed try again later");

            // Add new password
            var addResult = await _userManager.AddPasswordAsync(user, newPassword);

            if (!addResult.Succeeded)
                return Error.Failure("password reset failed try again later");

            return Result.Success;
        }

        public async Task<ErrorOr<Success>> ConfirmEmail(string email)
        {
           
            var user = await _userManager.FindByEmailAsync(email);

           

            user.EmailConfirmed = true;

            return Result.Success;
        }

        public async Task<ErrorOr<Success>> ActivateUser(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null )
                return Error.NotFound("User_Not_Found", $"User with Id {(id)} not found");

            if (user.IsActive)
                return Error.Conflict("User_Already_Active", $"User With {(id)} already active");

            user.Activate();

            return Result.Success;
        }

        public async Task<ErrorOr<Success>> DeActivateUser(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null)
                return Error.NotFound("User_Not_Found", $"User with Id {(id)} not found");

            if (!user.IsActive)
                return Error.Conflict("User_Already_InActive", $"User With {(id)} already inactive");

            user.Deactivate();

            return Result.Success;
        }

        public async Task<PaginatedList<IdentityUser>> GetAccountsAsync(int page, int pageSize, string? searchTearm, string? role, bool? isActive, CancellationToken cancellationToken)
        {



            var users = _appDbContext.Users.AsQueryable();

            // Apply filters
            if (isActive.HasValue)
                users = users.Where(u => u.IsActive == isActive.Value);

            if (!string.IsNullOrWhiteSpace(searchTearm))
            {
                var st = searchTearm.Trim();
                users = users.Where(u =>
                    EF.Functions.Like(u.Email, $"%{st}%") ||
                    EF.Functions.Like(u.FirstName, $"%{st}%") ||
                    EF.Functions.Like(u.LastName, $"%{st}%"));
            }

            // Apply role filter by checking if user has the role
            if (!string.IsNullOrWhiteSpace(role))
            {
                users = users.Where(u => _appDbContext.UserRoles
                    .Where(ur => ur.UserId == u.Id)
                    .Join(_appDbContext.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                    .Contains(role));
            }

            // Get total count
            var totalCount = await users.CountAsync(cancellationToken);

            // Get paginated users with their roles
            var pagedUsers = await users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            // Get roles for these users
            var userIds = pagedUsers.Select(u => u.Id).ToList();

            var userRoles = await _appDbContext.UserRoles
                .Where(ur => userIds.Contains(ur.UserId))
                .Join(_appDbContext.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, RoleName = r.Name })
                .GroupBy(x => x.UserId)
                .ToDictionaryAsync(g => g.Key, g => g.Select(x => x.RoleName).ToList(), cancellationToken);

            // Build IdentityUser objects
            var items = pagedUsers.Select(user => new IdentityUser(
                user.Id,
                user.Email,
                userRoles.GetValueOrDefault(user.Id) ?? new List<string>(),
                user.PhoneNumber,
                user.FirstName,
                user.LastName,
                user.PhotoUrl,
                user.IsActive,
                user.EmailConfirmed
            )).ToList();

            return new PaginatedList<IdentityUser>
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Items = items
            };
        }
   
        public async Task<bool> UserExistsAsync(string email , CancellationToken cancellationToken )
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == email , cancellationToken);

            if (user == null) 
                return false;
            return true;
        }
    }
}
using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Authentication.Dtos;
using Eghatha.Domain.Identity;
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

        Task<ErrorOr<IdentityUser>> GetUserDetailsByIdAsync(Guid userId, CancellationToken cancellationToken);

        Task<ErrorOr<IdentityUser>> GetIdentityUserByEmailAsync(string email, CancellationToken cancellationToken);

        Task<ErrorOr<Success>> ResetPasswordAsync(string email, string newPassword, CancellationToken cancellationToken);

        Task<ErrorOr<Success>> ConfirmEmail(string email);

        Task<ErrorOr<Success>> DeActivateUser(Guid id);

        Task<ErrorOr<Success>> ActivateUser(Guid id);

        Task<PaginatedList<IdentityUser>> GetAccountsAsync(int page, int pageSize, string? searchTearm, string? role, bool? isActive, CancellationToken cancellationToken);

        Task<ErrorOr<Guid>> CreatUserAsync(string firstName, string lastName, string email, string phoneNumber, string? password, string photoUrl, UserCreationMode mode);

        Task<ErrorOr<Success>> AddUserToRoleAsync(Guid userId, Role role);
        Task<bool> UserExistsAsync(string email, CancellationToken cancellationToken);
    }
}

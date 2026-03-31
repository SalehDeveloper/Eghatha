using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Models;
using Eghatha.Contract.Identity.Responses;

namespace Eghatha.Api.Mappers
{
    public static class IdentityMapper
    {
        public static UserResponse ToUserResponse(this AppUserDto userDto)
        {
            return new UserResponse(
                userDto.UserId,
                userDto.Email,
                userDto.Roles);
        }

        public static MeResponse ToMeResponse(this IdentityUser user)
        {
            return new MeResponse(
                user.Id,
                user.Email,
                user.Roles,
                user.PhoneNumber,
                user.FirstName,
                user.LastName,
                user.PhotoUrl);
        }
    }
}

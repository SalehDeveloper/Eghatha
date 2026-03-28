using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Features.Authentication.Dtos;
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

        public static MeResponse ToMeResponse(this MeDto me)
        {
            return new MeResponse(
                me.Id,
                me.Email,
                me.Roles,
                me.PhoneNumber,
                me.FirstName,
                me.LastName,
                me.PhotoUrl);
        }
    }
}

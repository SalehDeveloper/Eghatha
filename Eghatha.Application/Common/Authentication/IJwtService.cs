using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Features.Authentication.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Authentication
{
    public interface  IJwtService
    {
        public AccessTokenDto GenerateAccessToken(AppUserDto user);
        public string GenerateRefreshToken();

       public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);


    }
}

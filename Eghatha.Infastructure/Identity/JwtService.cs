using Azure.Core;
using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Features.Authentication.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Identity
{
    public class JwtService : IJwtService
    {
      

        private readonly AuthenticationOptions _options;
        private readonly TimeProvider _timeProvider;
        

        public JwtService(IOptions<AuthenticationOptions> options, TimeProvider timeProvider)
        {
            _options = options.Value;
            _timeProvider = timeProvider;
           
        }

        public AccessTokenDto GenerateAccessToken(AppUserDto user)
        {
            var claims = new List<Claim>
            {
                  new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                  new Claim(JwtRegisteredClaimNames.Email, user.Email),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                  new Claim("userId" , user.UserId.ToString())


            };
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(

                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: _timeProvider.GetUtcNow().AddMinutes(_options.AccessTokenDurationInMinutes).UtcDateTime,
                signingCredentials: signingCredentials
                );


            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new AccessTokenDto(accessToken, _timeProvider.GetUtcNow().AddMinutes(_options.AccessTokenDurationInMinutes).UtcDateTime);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key)),
                ValidateIssuer = true,
                ValidIssuer = _options.Issuer,
                ValidateAudience = true,
                ValidAudience = _options.Audience,
                ValidateLifetime = false, // Ignore token expiration
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token.");
            }

            return principal;
        }


    }
}
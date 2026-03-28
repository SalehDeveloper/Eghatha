using Eghatha.Application.Common.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Identity
{
    public class CookieService : ICookieService
    {
        private const string AccessTokenCookieName = "accessToken";
        private const string RefreshTokenCookieName = "refreshToken";

        private readonly AuthenticationOptions _options;
        private readonly TimeProvider _timeProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieService(
            IOptions< AuthenticationOptions> options,
            TimeProvider timeProvider,
            IHttpContextAccessor httpContextAccessor)
        {
            _options = options.Value;
            _timeProvider = timeProvider;
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetAccessTokenInCookies(string accessToken)
        {
            var accessTokenOptions = new CookieOptions
            {

                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = _timeProvider.GetUtcNow().AddMinutes(_options.AccessTokenDurationInMinutes).UtcDateTime
            };

            var context = _httpContextAccessor.HttpContext;
            if (context is null)
                throw new InvalidOperationException("HttpContext is not available.");
            context.Response.Cookies.Append(AccessTokenCookieName, accessToken, accessTokenOptions);
        }

        public void SetRefreshTokenInCookies(string refreshToken)
        {
            var refreshTokenOptions = new CookieOptions
            {

                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = _timeProvider.GetUtcNow().AddDays(_options.RefreshTokenDurationInDays).UtcDateTime
            };

            var context = _httpContextAccessor.HttpContext;

            if (context is null)
                throw new InvalidOperationException("HttpContext is not available.");

            context.Response.Cookies.Append(RefreshTokenCookieName, refreshToken, refreshTokenOptions);
        }

        public string? GetAccessToken()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context is null)
                throw new InvalidOperationException("HttpContext is not available.");

            context.Request.Cookies.TryGetValue(AccessTokenCookieName, out var token);

            return token;
        }

        public string? GetRefreshToken()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context is null)
                throw new InvalidOperationException("HttpContext is not available.");

            context.Request.Cookies.TryGetValue(RefreshTokenCookieName, out var token);

            return token;
        }

        public void DeleteAccessToken()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context is null)
                throw new InvalidOperationException("HttpContext is not available.");

            context.Response.Cookies.Append(AccessTokenCookieName, "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(-1)
            });
        }

        public void DeleteRefreshToken()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context is null)
                throw new InvalidOperationException("HttpContext is not available.");

            context.Response.Cookies.Append(RefreshTokenCookieName, "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(-1)
            });
        }



    }
}


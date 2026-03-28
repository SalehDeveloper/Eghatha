using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Authentication
{
    public interface  ICookieService
    {
        public void SetAccessTokenInCookies(string accessToken);
        public void SetRefreshTokenInCookies(string refreshToken);

       public string? GetAccessToken();
       public string? GetRefreshToken();

       public void DeleteAccessToken();
        public void DeleteRefreshToken();

    }
}

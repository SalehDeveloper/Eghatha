using Eghatha.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Authentication
{
    public interface  IJwtService
    {
        public string GenerateAccessToken(AppUserDto user);
        public string GenerateRefreshToken();
       
    }
}

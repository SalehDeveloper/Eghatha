using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Identity
{
    public class AuthenticationOptions
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int AccessTokenDurationInMinutes { get; set; }
        public int RefreshTokenDurationInDays { get; set; }

  
    }
}

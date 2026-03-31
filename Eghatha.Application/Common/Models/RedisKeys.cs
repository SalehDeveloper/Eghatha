using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Models
{
    public static class RedisKeys
    {
        public static class ConfirmEmail
        {
            private const string Prefix = "otp:email:";

            public static string Code(string email)
         => $"{Prefix}{email}";

            public static string RateLimit(string email)
                => $"{Prefix}{email}:rl";
        }

        public static class ResetPassword
        {
            private const string Prefix = "otp:password:";
            public static string Code(string email)
                => $"{Prefix}{email}";
            public static string RateLimit(string email)
                => $"{Prefix}{email}:rl";
        }
    }
}

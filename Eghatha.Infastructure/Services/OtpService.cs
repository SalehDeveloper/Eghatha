using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Common.Services;
using ErrorOr;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Services
{
    internal class OtpService : IOtpService
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly IOtpCodeGenerator _otpCodeGenerator;

        public OtpService(IRedisCacheService redisCacheService, IOtpCodeGenerator otpCodeGenerator)
        {
            _redisCacheService = redisCacheService;
            _otpCodeGenerator = otpCodeGenerator;
        }

        private static string GetKey(OtpType type, string email)
       => type switch
       {
           OtpType.ConfirmEmail => RedisKeys.ConfirmEmail.Code(email),
           OtpType.ResetPassword => RedisKeys.ResetPassword.Code(email),
           _ => throw new ArgumentOutOfRangeException()
       };

        private static string GetRateLimitKey(OtpType type, string email)
            => type switch
            {
                OtpType.ConfirmEmail => RedisKeys.ConfirmEmail.RateLimit(email),
                OtpType.ResetPassword => RedisKeys.ResetPassword.RateLimit(email),
                _ => throw new ArgumentOutOfRangeException()
            };
      
        public async Task<ErrorOr<string>> RequestAsync(OtpType type, string email, TimeSpan ttl)
        {
            var key = GetKey(type, email);
            var rlKey = GetRateLimitKey(type, email);

            var rateLimit = await _redisCacheService.GetAsync(rlKey);
         
            if (rateLimit is not null)
                return Error.Conflict("Too_Many_Requests", "Please wait before requesting again.");

            var otp = _otpCodeGenerator.GenerateOtpCode();

            // overrid the old otp if exists and set new ttl
            await _redisCacheService.SetAsync(key, otp, ttl);

            await _redisCacheService.SetAsync(rlKey, "1", TimeSpan.FromMinutes(5));

            return otp;
        }

        public async Task<ErrorOr<Success>> ValidateAsync(OtpType type, string email, string otp)
        {
            var key = GetKey(type, email);

            var stored = await _redisCacheService.GetAsync(key);

            if (stored is null || stored != otp)
               return Error.Conflict(
          code: "Auth.InvalidOtp",
          description: "Otp code is wrong or expired.");

            return Result.Success;
        }
        public async Task RemoveAsync(OtpType type, string email)
        {
            var key = GetKey(type, email);
            await _redisCacheService.RemoveAsync(key);
        }

      

      
    }
}

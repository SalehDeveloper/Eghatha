using Eghatha.Application.Common.Models;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Services
{
    public interface IOtpService
    {
        Task<ErrorOr<string>> RequestAsync(OtpType type, string email, TimeSpan ttl);

        Task<ErrorOr<Success>> ValidateAsync(OtpType type, string email, string otp);

        Task RemoveAsync(OtpType type, string email);
    }
}

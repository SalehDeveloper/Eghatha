using Eghatha.Application.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Services
{
    public class OtpCodeGenerator : IOtpCodeGenerator
    {
        public string GenerateOtpCode()
        {
            byte[] randomBytes = new byte[4];

            // Generate cryptographically secure random bytes
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            // Convert bytes to a uint and limit the result to 8 digits
            uint randomNumber = BitConverter.ToUInt32(randomBytes, 0) % 100000000;

            // Format as an 8-digit number with leading zeros if necessary
            return randomNumber.ToString("D8");
        }
    }
}

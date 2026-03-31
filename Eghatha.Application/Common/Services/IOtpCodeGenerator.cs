using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Services
{
    public interface IOtpCodeGenerator
    {
        public string GenerateOtpCode();
    }
}

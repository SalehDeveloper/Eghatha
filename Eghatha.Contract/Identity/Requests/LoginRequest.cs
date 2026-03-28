using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Identity.Requests
{
    public sealed record LoginRequest(string Email , string Password);


}

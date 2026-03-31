using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Identity.Requests
{
    public record ConfirmEmailRequest(string Email, string Otp);


}

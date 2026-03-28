using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Identity.Responses
{
    public sealed record UserResponse(Guid UserId, string Email, IList<string> Roles);


}

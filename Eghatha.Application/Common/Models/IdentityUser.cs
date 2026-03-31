using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Models
{
    public record IdentityUser(Guid Id ,string Email ,IList<string> Roles, string PhoneNumber, string FirstName, string LastName, string PhotoUrl , bool IsActive , bool IsEmailConfirmed);



}

using Eghatha.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Authentication.Dtos
{
    public sealed record MeDto(Guid Id , string Email ,IList<string> Roles ,string PhoneNumber, string FirstName , string LastName ,string PhotoUrl);
    
    
}

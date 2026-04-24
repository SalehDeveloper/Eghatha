using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.VolunteerRegisterations.Requests
{
    public record GetVolunteerRegisteartionsFilter(string? SearchTerm, string? Status);
    
    
}

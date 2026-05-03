using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Volunteers.Responses
{
    public record VolunteerEquipmentResponse(Guid Id,
    string Name,
    string Category,
    int Quantity,
    string Status);
    
    
}

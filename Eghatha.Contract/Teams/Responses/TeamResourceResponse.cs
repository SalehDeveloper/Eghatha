using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Teams.Responses
{
    public record TeamResourceResponse(
        Guid Id,
    string Type,
    int Quantity,
    string Status,
    bool IsConsumable);
    
    
}

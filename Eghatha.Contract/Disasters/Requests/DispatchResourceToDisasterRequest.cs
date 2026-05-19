using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Disasters.Requests
{
    public sealed record  DispatchResourceToDisasterRequest(Guid TeamId,
    Guid ResourceId,
    int Quantity,
    string? Notes);
    
    
    
}

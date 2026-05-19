using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Disasters.Responses
{
    public sealed record DisasterResponse(Guid Id,
     string Title,
     string City,
     string Province,
     string Type,
     string Status,
     DateTimeOffset StartTime
        );
    
}

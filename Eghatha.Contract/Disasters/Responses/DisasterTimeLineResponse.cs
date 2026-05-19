using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Disasters.Responses
{
    public sealed record DisasterTimeLineResponse(Guid Id,
    string EventType,
    string Description,
    DateTimeOffset OccurredAt);
    
    
}

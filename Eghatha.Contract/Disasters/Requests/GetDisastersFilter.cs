using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Disasters.Requests
{
    public sealed record GetDisastersFilter(string? City,
     string? Province,
     string? Type,
     string? Status,
     DateTimeOffset? From,
     DateTimeOffset? To);
    
    
}

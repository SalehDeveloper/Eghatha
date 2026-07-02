using Eghatha.Domain.Disasters.DisasterResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Disasters
{
    public record class DispatchResourceResult(DisasterResource Resource, bool IsNew);
    
    
}

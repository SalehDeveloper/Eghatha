using Ardalis.SmartEnum;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Disasters.DisasterResources
{
    public static class DisasterResourceErrors
    {
       public static readonly Error ResourceConsumptionExceedsSent = Error.Conflict(
            code: "DisasterResourceErrors.ResourceConsumptionExceedsSent",
            description: "resource consumption exceeds sent quantity");
    
        
        public static readonly Error InvalidReturnQuantity = Error.Conflict(
            code: "DisasterResourceErrors.InvalidReturnQuantity",
            description: "return quantity cannot exceed remaining quantity");


        public static readonly Error InvalidDamagedQuantity = Error.Conflict(
            code: "DisasterResourceErrors.InvalidDamagedQuantity",
            description: "damaged quantity cannot exceed remaining quantity");
    }
}

using ErrorOr;

namespace Eghatha.Domain.Disasters.DisasterResources
{
    public static class DisasterResourceErrors
    {
        public static readonly Error ResourceConsumptionExceedsSent = Error.Conflict(
             code: "DisasterResourceErrors.ResourceConsumptionExceedsSent",
             description: Resources.DisasterErrors.DisasterErrors_ResourceConsumptionExceedsSent);


        public static readonly Error InvalidReturnQuantity = Error.Conflict(
            code: "DisasterResourceErrors.InvalidReturnQuantity",
            description: Resources.DisasterErrors.DisasterErrors_InvalidReturnQuantity);


        public static readonly Error InvalidDamagedQuantity = Error.Conflict(
            code: "DisasterResourceErrors.InvalidDamagedQuantity",
            description: Resources.DisasterErrors.DisasterErrors_InvalidDamagedQuantity);

        public static readonly Error ResourceNotFound = Error.NotFound(
            code: "DisasterResourceErrors.ResourceNotFound",
            description: Resources.DisasterErrors.DisasterErrors_ResourceNotFound);

        public static readonly Error ResourceIsNotConsumable = Error.Conflict(
            code: "DisasterResourceErrors.ResourceIsNotConsumable",
            description: Resources.DisasterErrors.DisasterErrors_ResourceIsNotConsumable);
    }
}
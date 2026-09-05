using ErrorOr;

namespace Eghatha.Domain.Teams.TeamResources
{
    public static class ResourceErrors
    {
        public static readonly Error InvalidResourceType = Error.Validation(
            code: "TeamErrors.Resource.InvalidType",
            description: Resources.TeamErrors.TeamErrors_Resource_InvalidType
        );

        public static readonly Error ResourceTypeRequired = Error.Validation(
            code: "TeamErrors.Resource.TypeRequired",
            description: Resources.TeamErrors.TeamErrors_Resource_TypeRequired
        );

        public static readonly Error StatusRequired = Error.Validation(
            code: "TeamErrors.Resource.StatusRequired",
            description: Resources.TeamErrors.TeamErrors_Resource_StatusRequired
        );


        public static readonly Error QuantityShouldBeGreaterThanZero = Error.Validation(
            code: "TeamErrors.Resource.QuantityInvalid",
            description: Resources.TeamErrors.TeamErrors_Resource_QuantityInvalid);

        public static readonly Error NotEnoughResources = Error.Conflict(
            code: "TeamErrors.Resource.NotEnough",
            description: Resources.TeamErrors.TeamErrors_Resource_NotEnough


        );

        public static Error NotFound = Error.NotFound(
            code: "TeamErrors.Resource.NotFound",
            description: Resources.TeamErrors.TeamErrors_Resource_NotFound
        );
    }
}

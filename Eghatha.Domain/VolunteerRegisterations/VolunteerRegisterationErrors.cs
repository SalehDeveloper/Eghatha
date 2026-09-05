using ErrorOr;

namespace Eghatha.Domain.VolunteerRegisterations
{
    public static class VolunteerRegisterationErrors
    {
        public static readonly Error AlreadyProcessed = Error.Conflict(
            code: "VolunteerRegisterationErrors.AlreadyProcessed",
            description: Resources.VolunteerRegisterationErrors.VolunteerRegisterationErrors_AlreadyProcessed);
    }
}

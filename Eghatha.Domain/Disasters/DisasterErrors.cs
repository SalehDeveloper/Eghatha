using ErrorOr;

namespace Eghatha.Domain.Disasters
{
    public static class DisasterErrors
    {
        public static Error ReporterNameRequired = Error.Validation(
            code: "DisasterErrors.ReporterNameRequired",
            description: Resources.DisasterErrors.DisasterErrors_ReporterNameRequired);

        public static Error ReporterContactRequired = Error.Validation(
            code: "DisasterErrors.ReporterContactRequired",
            description: Resources.DisasterErrors.DisasterErrors_ReporterContactRequired);

        public static Error ReporterIdRequired = Error.Validation(
              code: "DisasterErrors.ReporterIdRequired",
            description: Resources.DisasterErrors.DisasterErrors_ReporterIdRequired);

        public static Error TitleRequired = Error.Validation(
           code: "DisasterErrors.TitleRequired",
            description: Resources.DisasterErrors.DisasterErrors_TitleRequired);

        public static Error DescriptionRequired = Error.Validation(
        code: "DisasterErrors.DescriptionRequired",
         description: Resources.DisasterErrors.DisasterErrors_DescriptionRequired);

        public static Error LocationRequired = Error.Validation(
        code: "DisasterErrors.LocationRequired",
         description: Resources.DisasterErrors.DisasterErrors_LocationRequired);

        public static Error ReporterInfoRequired = Error.Validation(
       code: "DisasterErrors.ReporterInfoRequired",
        description: Resources.DisasterErrors.DisasterErrors_ReporterInfoRequired);

        public static Error CustomTypeDescriptionRequired = Error.Validation(
            code: "DisasterErrors.CustomTypeDescriptionRequired",
        description: Resources.DisasterErrors.DisasterErrors_CustomTypeDescriptionRequired);

        public static Error InvalidStatusTransition(DisasterStatus current, DisasterStatus next) => Error.Conflict(
        code: "DisasterErrors.InvalidStatusTransition",
        description: $"Disaster Invalid Status transition from '{current}' to '{next}'.");

        public static Error InvalidScore = Error.Validation(
            code: "DisasterErrors.InvalidParticipantScore",
            description: Resources.DisasterErrors.DisasterErrors_InvalidParticipantScore);

        public static Error VolunteerAlreadyAssigned = Error.Conflict(
            code: "DisasterErrors.VolunteerAlreadyAssigned",
            description: Resources.DisasterErrors.DisasterErrors_VolunteerAlreadyAssigned);

        public static Error CannotAssignVolunteerWhenNotInValidStatus = Error.Conflict(
            code: "DisasterErrors.CannotAssignVolunteerWhenNotInValidStatus",
            description: Resources.DisasterErrors.DisasterErrors_CannotAssignVolunteerWhenNotInValidStatus);


        public static Error CannotAssignResourceWhenNotInValidStatus = Error.Conflict(
               code: "DisasterErrors.CannotAssignResourceWhenNotInValidStatus",
               description: Resources.DisasterErrors.DisasterErrors_CannotAssignResourceWhenNotInValidStatus);

        public static Error CannotRemoveVolunteerWhenNotInReportedStatus = Error.Conflict(
           code: "DisasterErrors.CannotRemoveVolunteerWhenNotReported",
           description: Resources.DisasterErrors.DisasterErrors_CannotRemoveVolunteerWhenNotReported);

        public static Error CannotAddAffectedPersonsWhenDisasterNotResolved = Error.Conflict(
            code: "DisasterErrors.CannotAddAffectedPersonsWhenNotResolved",
            description: Resources.DisasterErrors.DisasterErrors_CannotAddAffectedPersonsWhenNotResolved);

        public static Error CannotUpdateAffectedPersonsWhenDisasterNotResolved = Error.Conflict(
           code: "DisasterErrors.CannotUpdateAffectedPersonsWhenDisasterNotResolved",
           description: Resources.DisasterErrors.DisasterErrors_CannotUpdateAffectedPersonsWhenDisasterNotResolved);

        public static Error CannotGenerateReportWhenDisasterNotClosed = Error.Conflict(
            code: "DisasterErrors.CannotGenerateReportWhenDisasterNotClosed",
            description: Resources.DisasterErrors.DisasterErrors_CannotGenerateReportWhenDisasterNotClosed);


        public static Error volunteerNotFound = Error.NotFound(
            code: "DisasterErrors.Volunteer.NotFound",
            description: Resources.DisasterErrors.DisasterErrors_Volunteer_NotFound);

        public static Error ResourceQuantityshouldBeGreaterThanZero = Error.Validation(
            code: "DisasterErrors.ResourceQuantityshouldBeGreaterThanZero",
            description: Resources.DisasterErrors.DisasterErrors_ResourceQuantityshouldBeGreaterThanZero);

        public static Error AffectedPeronNotFound = Error.NotFound(
            code: "DisasterErrors.AffectedPerson.NotFound",
            description: Resources.DisasterErrors.DisasterErrors_AffectedPerson_NotFound);

        public static Error ReportAlreadyExists = Error.Conflict(
            code: "DisasterErrors.ReportAlreadyExist",
            description: Resources.DisasterErrors.DisasterErrors_ReportAlreadyExist);

        public static Error CannotAssignTeamWhenNotInValidStatus = Error.Conflict(
          code: "DisasterErrors.CannotAssignTeamWhenNotInValidStatus",
          description: Resources.DisasterErrors.DisasterErrors_CannotAssignTeamWhenNotInValidStatus);

        public static Error TeamAlreadyAssigned = Error.Conflict(
          code: "DisasterErrors.TeamAlreadyAssigned",
          description: Resources.DisasterErrors.DisasterErrors_TeamAlreadyAssigned);

        public static Error TeamNotFound = Error.NotFound(
          code: "DisasterErrors.Team.NotFound",
          description: Resources.DisasterErrors.DisasterErrors_Team_NotFound);

        public static Error InvalidType = Error.Validation(
        code: "DisasterErrors.Type.Invalid",
        description: Resources.DisasterErrors.DisasterErrors_Type_Invalid);

        public static Error ProvinceRequired = Error.Validation(
            code: "DisasterErrors.ProvinceRequired",
            description: Resources.DisasterErrors.DisasterErrors_ProvinceRequired);

        public static Error CityRequired = Error.Validation(
          code: "DisasterErrors.CityRequired",
          description: Resources.DisasterErrors.DisasterErrors_CityRequired);

        public static Error TeamNotAssignedToDisaster = Error.Conflict(
            code: "DisasterErrors.TeamNotAssignedToDisaster",
            description: Resources.DisasterErrors.DisasterErrors_TeamNotAssignedToDisaster);

        public static Error InvalidStatus = Error.Validation(
             code: "DisasterErrors.InvalidStatus",
             description: Resources.DisasterErrors.DisasterErrors_InvalidStatus);

        public static Error FailedToAssign = Error.Conflict(
            code: "DisasterErrors.FailedToAssign",
            description: Resources.DisasterErrors.DisasterErrors_FailedToAssign);

        public static Error CannotArchiveWithoutReport = Error.Conflict(
            code: "DisasterErrors.CannotArchiveWithoutReport",
            description: Resources.DisasterErrors.DisasterErrors_CannotArchiveWithoutReport);

        public static Error CannotCloseDisasterWithUnevaluatedVolunteers = Error.Conflict(
             code: "DisasterErrors.CannotCloseDisasterWithUnevaluatedVolunteers",
            description: Resources.DisasterErrors.DisasterErrors_CannotCloseDisasterWithUnevaluatedVolunteers);

        public static Error CannotCloseDisasterWithUnmanagedResources = Error.Conflict(
             code: "DisasterErrors.CannotCloseDisasterWithUnmanagedResources",
            description: Resources.DisasterErrors.DisasterErrors_CannotCloseDisasterWithUnmanagedResources);

    }
}
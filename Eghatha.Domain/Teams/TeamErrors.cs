using ErrorOr;

namespace Eghatha.Domain.Teams
{
    public static class TeamErrors
    {
        public static Error NameRequired = Error.Validation(
            code: "TeamErrors.Name.Required",
            description: Resources.TeamErrors.TeamErrors_Name_Required);


        public static Error SpecialityRequired = Error.Validation(
            code: "TeamErrors.Speciality.Required",
            description: Resources.TeamErrors.TeamErrors_Speciality_Required);

        public static Error ProvinceRequired = Error.Validation(
            code: "TeamErrors.Province.Required",
            description: Resources.TeamErrors.TeamErrors_Province_Required);

        public static Error CityRequired = Error.Validation(
            code: "TeamErrors.City.Required",
            description: Resources.TeamErrors.TeamErrors_City_Required);

        public static Error LocationRequired = Error.Validation(
            code: "TeamErrors.Location.Required",
            description: Resources.TeamErrors.TeamErrors_Location_Required);

        public static Error LocationMustBeInsyria = Error.Validation(
          code: "TeamErrors.Location.MustBeInSyria",
          description: Resources.TeamErrors.TeamErrors_Location_MustBeInSyria);

        public static Error CreatedByAdminIdRequired = Error.Validation(
           code: "TeamErrors.AdminId.Required",
           description: Resources.TeamErrors.TeamErrors_AdminId_Required);

        public static Error InvalidSpeciality = Error.Validation(
            code: "TeamErrors.Speciality.Invalid",
            description: Resources.TeamErrors.TeamErrors_Speciality_Invalid);

        public static Error CannotSetToActiveWhenInMission = Error.Conflict(
            code: "TeamErrors.Status.CannotSetToActiveWhenInMission",
            description: Resources.TeamErrors.TeamErrors_Status_CannotSetToActiveWhenInMission);

        public static Error TeamAlreadyHasLeader = Error.Conflict(
            code: "TeamErrors.AlreadyHasLeader",
            description: Resources.TeamErrors.TeamErrors_AlreadyHasLeader);

        public static Error CannotRemoveMemberWhenInMission = Error.Conflict(
            code: "TeamErrors.CannotRemoveMemberWhenInMission",
            description: Resources.TeamErrors.TeamErrors_CannotRemoveMemberWhenInMission);

        public static Error MemberNotFound = Error.NotFound(
            code: "TeamErrors.MemberNotFound",
            description: Resources.TeamErrors.TeamErrors_MemberNotFound);

        public static Error CannotRemoveLeader = Error.Conflict(
             code: "TeamErrors.CannotRemoveLeader",
             description: Resources.TeamErrors.TeamErrors_CannotRemoveLeader);

        public static Error StatusRequired = Error.Validation(
            code: "TeamErrors.Status.Required",
            description: Resources.TeamErrors.TeamErrors_Status_Required);

        public static Error InvalidStatus = Error.Validation(
            code: "TeamErrors.Status.Invalid",
            description: Resources.TeamErrors.TeamErrors_Status_Invalid);

        public static Error MemberMustBeActiveToBecomeLeader =
        Error.Conflict("TeamErrors.MemberNotActive", Resources.TeamErrors.TeamErrors_MemberNotActive);

        public static Error InvalidStatusTransition(TeamStatus current, TeamStatus next) => Error.Conflict(
 code: "TeamErrors.InvalidStatusTransition",
 description: $"Team Invalid Status transition from '{current}' to '{next}'.");

    }
}

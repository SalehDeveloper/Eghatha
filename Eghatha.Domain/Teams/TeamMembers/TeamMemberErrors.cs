using Ardalis.SmartEnum;
using ErrorOr;

namespace Eghatha.Domain.Teams.TeamMembers
{
    public class TeamMemberErrors : SmartEnum<TeamMemberErrors>
    {
        public static readonly Error JobTitleRequired = Error.Validation(
            code: "TeamErrors.Member.JobTitleRequired",
            description: Resources.TeamErrors.TeamErrors_Member_JobTitleRequired);

        public static Error CannotSetToActiveWhenInMission = Error.Conflict(
       code: "TeamErrors.Member.Status.CannotSetToActiveWhenInMission",
       description: Resources.TeamErrors.TeamErrors_Member_Status_CannotSetToActiveWhenInMission);


        public static Error StatusRequired = Error.Validation(
            code: "TeamErrors.Member.Status.Required",
            description: Resources.TeamErrors.TeamErrors_Member_Status_Required);

        public static Error InvalidStatus = Error.Validation(
           code: "TeamErrors.Member.Status.Invalid",
           description: Resources.TeamErrors.TeamErrors_Member_Status_Invalid);

        public static Error InvalidStatusTransition(TeamMemberStatus current, TeamMemberStatus next) => Error.Conflict(
   code: "TeamErrors.Member.InvalidStatusTransition",
   description: $"Team-Member Invalid Status transition from '{current}' to '{next}'.");

        public TeamMemberErrors(string name, int value) : base(name, value)
        {
        }
    }
}

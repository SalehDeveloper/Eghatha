using Ardalis.SmartEnum;
using Eghatha.Domain.Disasters;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Teams.TeamMembers
{
    public class TeamMemberErrors : SmartEnum<TeamMemberErrors>
    {
        public static readonly Error JobTitleRequired= Error.Validation(
            code: "TeamErrors.Member.JobTitleRequired",
            description: "Job title is required.");

        public static Error CannotSetToActiveWhenInMission = Error.Conflict(
       code: "TeamErrors.Member.Status.CannotSetToActiveWhenInMission",
       description: "Cannot set team-member status to active when it's on a mission.");


        public static Error StatusRequired = Error.Validation(
            code: "TeamErrors.Member.Status.Required",
            description: "Team-Member status is required.");

        public static Error InvalidStatus = Error.Validation(
           code: "TeamErrors.Member.Status.Invalid",
           description: "Team-Member status is invalid.");

        public static Error InvalidStatusTransition(TeamMemberStatus current, TeamMemberStatus next) => Error.Conflict(
   code: "TeamErrors.Member.InvalidStatusTransition",
   description: $"Team-Member Invalid Status transition from '{current}' to '{next}'.");

        public TeamMemberErrors(string name, int value) : base(name, value)
        {
        }
    }
}

using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Teams
{
    public static class TeamErrors
    {
        public static Error NameRequired = Error.Validation(
            code: "TeamErrors.Name.Required",
            description: "Team name is required.");


        public static Error SpecialityRequired = Error.Validation(
            code: "TeamErrors.Speciality.Required",
            description: "Team speciality is required.");

        public static Error ProvinceRequired = Error.Validation(
            code: "TeamErrors.Province.Required",
            description: "Team province is required.");

        public static Error CityRequired = Error.Validation(
            code: "TeamErrors.City.Required",
            description: "Team city is required.");

        public static Error LocationRequired = Error.Validation(
            code: "TeamErrors.Location.Required",
            description: "Team location is required.");

        public static Error CreatedByAdminIdRequired = Error.Validation(
           code: "TeamErrors.AdminId.Required",
           description: "id for  admin who created the team is required.");

        public static Error InvalidSpeciality = Error.Validation(
            code: "TeamErrors.Speciality.Invalid",
            description: "Team speciality is invalid.");

        public static Error CannotSetToActiveWhenInMission = Error.Conflict(
            code: "TeamErrors.Status.CannotSetToActiveWhenInMission",
            description: "Cannot set team status to active when it's on a mission.");
        
        public static Error TeamAlreadyHasLeader = Error.Conflict(
            code: "TeamErrors.AlreadyHasLeader",
            description: "Team already has a leader.");

        public static Error CannotRemoveMemberWhenInMission = Error.Conflict(
            code: "TeamErrors.CannotRemoveMemberWhenInMission",
            description: "Cannot remove team member when the team is on a mission.");

        public static Error MemberNotFound = Error.NotFound(
            code: "TeamErrors.MemberNotFound",
            description: "Team member not found.");

        public static Error CannotRemoveLeader = Error.Conflict(
             code:"TeamErrors.CannotRemoveLeader",
             description:"you cannot remove a leader member");

        public static Error StatusRequired = Error.Validation(
            code: "TeamErrors.Status.Required",
            description: "Team status is required.");

        public static Error InvalidStatus = Error.Validation(
            code: "TeamErrors.Status.Invalid",
            description: "Team status is invalid.");



    }
}

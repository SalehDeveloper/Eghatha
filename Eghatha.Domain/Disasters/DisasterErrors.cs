using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Disasters
{
    public static class DisasterErrors
    {
        public static Error ReporterNameRequired = Error.Validation(
            code: "DisasterErrors.ReporterNameRequired",
            description: "reporter name is required");

        public static Error ReporterContactRequired = Error.Validation(
            code: "DisasterErrors.ReporterContactRequired",
            description: "reporter contact is required");

        public static Error ReporterIdRequired = Error.Validation(
              code: "DisasterErrors.ReporterIdRequired",
            description: "reporter Id is required");

        public static Error TitleRequired = Error.Validation(
           code: "DisasterErrors.TitleRequired",
            description: "title is required");

        public static Error DescriptionRequired = Error.Validation(
        code: "DisasterErrors.DescriptionRequired",
         description: "description is required");

        public static Error LocationRequired = Error.Validation(
        code: "DisasterErrors.LocationRequired",
         description: "location is required");

        public static Error ReporterInfoRequired = Error.Validation(
       code: "DisasterErrors.ReporterInfoRequired",
        description: "reporter info is required");

        public static Error CustomTypeDescriptionRequired = Error.Validation(
            code: "DisasterErrors.CustomTypeDescriptionRequired",
        description: "description type is required");

        public static Error InvalidStatusTransition(DisasterStatus current, DisasterStatus next) => Error.Conflict(
        code: "DisasterErrors.InvalidStatusTransition",
        description: $"Disaster Invalid Status transition from '{current}' to '{next}'.");

        public static Error InvalidScore = Error.Validation(
            code: "DisasterErrors.InvalidParticipantScore",
            description: "participant Scores must be between 1 and 5");

        public static Error VolunteerAlreadyAssigned = Error.Conflict(
            code: "DisasterErrors.VolunteerAlreadyAssigned",
            description: "volunteer already assigned to disaster");

        public static Error CannotAssignVolunteerWhenNotInProgress = Error.Conflict(
            code: "DisasterErrors.CannotAssignVolunteerWhenNotInProgress",
            description:"you cannot assign a volunteer when disaster not in progress");


        public static Error CannotAssignResourceWhenNotInProgress = Error.Conflict(
            code: "DisasterErrors.CannotAssignResourceWhenNotInProgress",
            description: "you cannot assign resource when disaster not in progress");

        public static Error CannotRemoveVolunteerWhenNotInReportedStatus = Error.Conflict(
           code: "DisasterErrors.CannotRemoveVolunteerWhenNotReported",
           description: "you cannot remove a volunteer when disaster not in repoted status");

        public static Error CannotAddAffectedPersonsWhenDisasterNotResolved = Error.Conflict(
            code: "DisasterErrors.CannotAddAffectedPersonsWhenNotResolved",
            description: "you cannot add affected persons when disaster not in resolved status");

        public static Error CannotUpdateAffectedPersonsWhenDisasterNotResolved = Error.Conflict(
           code: "DisasterErrors.CannotUpdateAffectedPersonsWhenDisasterNotResolved",
           description: "you cannot updated affected persons when disaster not in resolved status");

        public static Error CannotGenerateReportWhenDisasterNotClosed = Error.Conflict(
            code: "DisasterErrors.CannotGenerateReportWhenDisasterNotClosed",
            description:"report cannot be generated if the disaster is not closed");


        public static Error volunteerNotFound = Error.NotFound(
            code: "DisasterErrors.Volunteer.NotFound",
            description:"volunteer is not assinged to this disaster");

        public static Error ResourceQuantityshouldBeGreaterThanZero = Error.Validation(
            code: "DisasterErrors.ResourceQuantityshouldBeGreaterThanZero",
            description:"resource allocated for a disaster should be greater then zero");

        public static Error AffectedPeronNotFound = Error.NotFound(
            code: "DisasterErrors.AffectedPerson.NotFound",
            description: "affected person no found in this disaster");

        public static Error ReportAlreadyExists = Error.Conflict(
            code: "DisasterErrors.ReportAlreadyExist",
            description: "disaster already has a report");


    }
}

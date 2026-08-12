using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.AiAssistant
{
    public static class ReportSchema
    {
        public const string Definition = """
    report.Disasters(Id, Title, Type[text], CustomTypeDescription, Status[numeric], City, Province, StartTime, EndTime, Description)
    report.AffectedPersons(Id, DisasterId, Name, Phone, Age, Status[text: Injured/Missing/Dead/Evacuated], Notes)
    report.DisasterVolunteers(Id, DisasterId, VolunteerId, EvaluatedAt, EvaluatedByLeaderId, Notes)
    report.Reports(Id, DisasterId, Summary, IssuedAt)
    report.Teams(Id, Name, Speciality[text], Status[numeric], City, Province)
    report.Resources(Id, TeamId, Type[text], Quantity, Status[text])
    report.TeamMembers(Id, TeamId, JobTitle, IsLeader, Status[text], JoinedAt, FirstName, LastName, Email)
    report.Volunteers(Id, Speciality[text], Status[text], YearsOfExperience, TotalMissions, TotalScore, City, Province, FirstName, LastName, Email)
    report.VolunteerEquipments(Id, VolunteerId, Category[text], Name, Quantity, Status[text])
    report.VolunteerRegistrations(Id, VolunteerId, Status[numeric], RequestedAt, ReviewedAt, RejectionReason)
    report.DisasterTeams(DisasterId, TeamId)
    """;
        public static readonly string[] AllowedViews =
  {
    "report.Disasters", "report.AffectedPersons", "report.DisasterVolunteers",
    "report.Reports", "report.Teams", "report.Resources", "report.TeamMembers",
    "report.Volunteers", "report.VolunteerEquipments", "report.VolunteerRegistrations",
    "report.DisasterTeams"
};
    }
}

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
        report.Disasters(Id, Title, Type, Status, City, Province, StartTime, EndTime, Description)
        report.AffectedPersons(Id, DisasterId, Age, Status)
        report.DisasterVolunteers(Id, DisasterId, VolunteerId, EvaluatedAt, Notes)
        report.Reports(Id, DisasterId, Summary, IssuedAt)
        report.Teams(Id, Name, City, Province, Speciality, Status)
        report.Resources(Id, TeamId, Type, Quantity, Status)
        report.TeamMembers(Id, TeamId, JobTitle, IsLeader, Status, JoinedAt)
        report.Volunteers(Id, City, Province, Speciality, Status, YearsOfExperience, TotalMissions, TotalScore, FirstName, LastName)
        report.VolunteerEquipments(Id, VolunteerId, Category, Name, Quantity, Status)
        report.VolunteerRegistrations(Id, VolunteerId, Status, RequestedAt, ReviewedAt)
        """;

        public static readonly string[] AllowedViews =
        {
        "report.Disasters",
        "report.AffectedPersons",
        "report.DisasterVolunteers",
        "report.Reports",
        "report.Teams",
        "report.Resources",
        "report.TeamMembers",
        "report.Volunteers",
        "report.VolunteerEquipments",
        "report.VolunteerRegistrations"
    };
    }
}

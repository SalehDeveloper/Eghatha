using Eghatha.Contract.Disasters.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Disasters.Responses
{
    public sealed record DisasterDetailsResponse(Guid Id,
    string Title,
    string Description,
    string City,
    string Province,
    string Type,
    string Status,
    double Latitude,
    double Longitude,
    DateTimeOffset StartTime,
    DateTimeOffset? EndTime,
    ReporterResponse Reporter,
    IReadOnlyList<DisasterTeamResponse> Teams,
    IReadOnlyList<ResourceResponse> Resources,
    IReadOnlyList<AffectedPersonResponse> AffectedPeople,
    ReportResponse? Report);

    public sealed record ReporterResponse(
   string Name,
   string Phone
);

    public sealed record DisasterTeamResponse(
   Guid TeamId,
   string? TeamName
);

    public sealed record ResourceResponse(
    Guid Id,
    string ResourceType,
    int Sent,
    int Consumed,
    int Returned,
    int Damaged,
    string? Notes
);

    public sealed record AffectedPersonResponse(
 string Name,
 int Age,
 string Phone,
 string Status,
 string? Notes);

    public sealed record ReportResponse(
     Guid Id,
     string Summary,
     string PdfUrl,
     DateTimeOffset IssuedAt
 );
}

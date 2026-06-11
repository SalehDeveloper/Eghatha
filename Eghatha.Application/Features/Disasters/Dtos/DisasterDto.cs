using Eghatha.Application.Features.Teams.Dtos;
using Eghatha.Domain.Disasters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Dtos
{
    public sealed record DisasterDto(
     Guid Id,
     string Title,
     string City,
     string Province,
     double Latitude,
     double Longitude,
     string Type,
     string Status,
     DateTimeOffset StartTime
 );


    public sealed record DisasterDetailsDto(
    Guid Id,
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
    ReporterDto Reporter,
    IReadOnlyList<TeamDto> Teams,
    IReadOnlyList<ResourceDto> Resources,
    IReadOnlyList<AffectedPersonDto> AffectedPeople,
    ReportDto? Report
);
}

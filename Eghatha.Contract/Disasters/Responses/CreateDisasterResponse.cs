using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Disasters.Responses
{
    public sealed record CreateDisasterResponse(
        Guid Id,
        string Status,
        IReadOnlyCollection<RecommendedTeamsResponse> RecommendedTeams,
        IReadOnlyCollection<RecommendedVolunteerResponse> RecommendedVolunteers);

    public sealed record RecommendedTeamsResponse(Guid TeamId,
    string TeamName,
    string Speciality,
    double DistanceKm,
    double DurationMinutes,
    double Score,
    bool IsLiveLocation);

    public sealed record RecommendedVolunteerResponse(Guid VolunteerId,
    string Speciality,
    double DistanceKm,
    double DurationMinutes,
    double Score);
    
    
}

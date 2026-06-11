using Eghatha.Domain.Teams;

namespace Eghatha.Application.Common.Models
{
    public sealed record RecommendedTeamDto(
    Guid TeamId,
    string TeamName,
    TeamSpeciality Speciality,
    string Province , 
    string City,
    double DistanceKm,
    double DurationMinutes,
    double Score,
    bool IsLiveLocation);
}

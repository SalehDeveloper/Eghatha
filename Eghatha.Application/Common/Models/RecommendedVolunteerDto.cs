using Eghatha.Domain.Volunteers;

namespace Eghatha.Application.Common.Models
{
    public sealed record RecommendedVolunteerDto(
    Guid VolunteerId,
    VolunteerSpeciality Speciality,
    double DistanceKm,
    double DurationMinutes,
    double Score);
}

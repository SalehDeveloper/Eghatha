namespace Eghatha.Application.Common.Models
{
    public sealed record RouteResult(
    Guid EntityId,
    double DistanceKm,
    double DurationMinutes);
}

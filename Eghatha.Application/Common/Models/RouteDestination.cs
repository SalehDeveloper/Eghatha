using Eghatha.Domain.Shared.ValueObjects;

namespace Eghatha.Application.Common.Models
{
    public sealed record RouteDestination(
    Guid EntityId,
    GeoLocation Location);
}

using Eghatha.Application.Common.Models;
using Eghatha.Domain.Shared.ValueObjects;

namespace Eghatha.Application.Common.Services
{
    public interface IRoutingService
    {
        Task<IReadOnlyList<RouteResult>> CalculateAsync(
            GeoLocation source,
            IReadOnlyList<RouteDestination> destinations,
            CancellationToken cancellationToken);
    }
}

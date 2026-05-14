using Eghatha.Application.Common.Models;
using Eghatha.Domain.Disasters;

namespace Eghatha.Application.Common.Services
{
    public interface ITeamRecommendationService
    {
        Task<IReadOnlyList<RecommendedTeamDto>> RecommendAsync(
            Disaster disaster,
            CancellationToken cancellationToken);
    }
}

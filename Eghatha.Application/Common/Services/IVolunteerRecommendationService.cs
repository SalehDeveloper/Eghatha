using Eghatha.Application.Common.Models;
using Eghatha.Domain.Disasters;

namespace Eghatha.Application.Common.Services
{
    public interface IVolunteerRecommendationService
    {
        Task<IReadOnlyList<RecommendedVolunteerDto>> RecommendAsync(
            Disaster disaster,
            CancellationToken cancellationToken);
    }
}

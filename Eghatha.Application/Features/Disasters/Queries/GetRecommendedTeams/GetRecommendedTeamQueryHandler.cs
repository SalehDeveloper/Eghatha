using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Common.Services;
using ErrorOr;
using MediatR;

namespace Eghatha.Application.Features.Disasters.Queries.GetRecommendedTeams
{
    public sealed class GetRecommendedTeamQueryHandler : IRequestHandler<GetRecommendedTeamQuery, ErrorOr<List<RecommendedTeamDto>>>
    { 
        private readonly ITeamRecommendationService _teamRecommendationService;
        private readonly IDisasterRepository _disasterRepository;

        public GetRecommendedTeamQueryHandler(ITeamRecommendationService teamRecommendationService, IDisasterRepository disasterRepository)
        {
            _teamRecommendationService = teamRecommendationService;
            _disasterRepository = disasterRepository;
        }

        public async Task<ErrorOr<List<RecommendedTeamDto>>> Handle(GetRecommendedTeamQuery request, CancellationToken cancellationToken)
        {


            var disaster = await _disasterRepository.GetByIdAsync(request.DisasterId , cancellationToken);

            if (disaster is null) return ApplicationErrors.DisasterNotFound;

            var recommendedTeams = await _teamRecommendationService.RecommendAsync(disaster, cancellationToken);

            return recommendedTeams.ToList();

        }
    }
}
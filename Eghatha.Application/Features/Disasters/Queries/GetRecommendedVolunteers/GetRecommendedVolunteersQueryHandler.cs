using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Common.Services;
using ErrorOr;
using MediatR;

namespace Eghatha.Application.Features.Disasters.Queries.GetRecommendedVolunteers
{
    public sealed class GetRecommendedVolunteersQueryHandler : IRequestHandler<GetRecommendedVolunteersQuery, ErrorOr<List<RecommendedVolunteerDto>>>
    {
        private readonly IDisasterRepository   _disasterRepository;
        private readonly IVolunteerRecommendationService _volunteerRecommendationService;

        public GetRecommendedVolunteersQueryHandler(IDisasterRepository disasterRepository, IVolunteerRecommendationService volunteerRecommendationService)
        {
            _disasterRepository = disasterRepository;
            _volunteerRecommendationService = volunteerRecommendationService;
        }

        public async Task<ErrorOr<List<RecommendedVolunteerDto>>> Handle(GetRecommendedVolunteersQuery request, CancellationToken cancellationToken)
        {
            var disaster = await _disasterRepository.GetByIdAsync(request.DisasterId, cancellationToken);

            if (disaster is null) return ApplicationErrors.DisasterNotFound;

            var recommendedVolunteers = await _volunteerRecommendationService.RecommendAsync(disaster, cancellationToken);

            return recommendedVolunteers.ToList();
        }
    }



}

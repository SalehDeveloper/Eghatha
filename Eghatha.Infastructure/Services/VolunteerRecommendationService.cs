using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Common.Services;
using Eghatha.Domain.Disasters;
using Eghatha.Domain.Volunteers.Equipments;

namespace Eghatha.Infastructure.Services
{
    public sealed class VolunteerRecommendationService
    : IVolunteerRecommendationService
    {
        private readonly IVolunteerRepository _volunteerRepository;
        private readonly IRoutingService _routingService;
        private readonly IVolunteerScoringService _volunteerScoringService;

        public VolunteerRecommendationService(
            IVolunteerRepository volunteerRepository,
            IRoutingService routingService,
            IVolunteerScoringService volunteerScoringService)
        {
            _volunteerRepository = volunteerRepository;
            _routingService = routingService;
            _volunteerScoringService = volunteerScoringService;
        }

        public async Task<IReadOnlyList<RecommendedVolunteerDto>> RecommendAsync(
            Disaster disaster,
            CancellationToken cancellationToken)
        {
            var volunteers = await _volunteerRepository.GetAvailableBySpecialitiesAsync(
                disaster.Type.RequiredVolunteerSpecialities,
                cancellationToken);

            if (volunteers.Count == 0)
                return [];

            var destinations = volunteers
                .Select(v => new RouteDestination(
                    v.Id,
                    v.Location))
                .ToList();

            var routes = await _routingService.CalculateAsync(
                disaster.Location,
                destinations,
                cancellationToken);

            var result = volunteers
                .Join(
                    routes,
                    volunteer => volunteer.Id,
                    route => route.EntityId,
                    (volunteer, route) =>
                    {

                        var score = _volunteerScoringService.Calculate(disaster, volunteer,route);
                        
                        return new RecommendedVolunteerDto(
                            volunteer.Id,
                            volunteer.Speciality,
                            Math.Round(route.DistanceKm, 1),
                            Math.Round(route.DurationMinutes, 0),
                            score);
                    })
                .OrderByDescending(x => x.Score)
                .Take(20)
                .ToList();

            return result;
        }
    }

}


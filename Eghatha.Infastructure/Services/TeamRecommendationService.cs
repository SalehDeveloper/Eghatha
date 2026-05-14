using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Common.Services;
using Eghatha.Domain.Disasters;
using Eghatha.Domain.Shared.ValueObjects;
using Eghatha.Domain.Teams;

namespace Eghatha.Infastructure.Services
{
    public sealed class TeamRecommendationService : ITeamRecommendationService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ITeamOperationalLocationProvider _locationProvider;
        private readonly IRoutingService _routingService;
        private readonly ITeamScoringService _scoringService;

        public TeamRecommendationService(
            ITeamRepository teamRepository,
            ITeamOperationalLocationProvider locationProvider,
            IRoutingService routingService,
            ITeamScoringService scoringService)
        {
            _teamRepository = teamRepository;
            _locationProvider = locationProvider;
            _routingService = routingService;
            _scoringService = scoringService;
        }
        public async Task<IReadOnlyList<RecommendedTeamDto>> RecommendAsync(Disaster disaster, CancellationToken cancellationToken)
        {
            var teams = await _teamRepository.GetAvailableTeamsAsync(
             disaster.Type.RecommendedTeamSpecialities,
             cancellationToken);

            var teamLocations = new List<(Team team, GeoLocation location, bool isLive)>();

            foreach (var team in teams)
            {
                var locationResult = await _locationProvider.GetLocationAsync(
                    team,
                    cancellationToken);

                teamLocations.Add((
                    team,
                    locationResult.location,
                    locationResult.isLiveLocation));
            }

            var destinations = teamLocations
                .Select(x => new RouteDestination(x.team.Id, x.location))
                .ToList();

            var routes = await _routingService.CalculateAsync(
                disaster.Location,
                destinations,
                cancellationToken);

            var result = new List<RecommendedTeamDto>();

            foreach (var route in routes)
            {
                var item = teamLocations.First(x => x.team.Id == route.EntityId);

                var score = _scoringService.Calculate(
                    item.team,
                    disaster,
                    route);

                result.Add(new RecommendedTeamDto(
                    item.team.Id,
                    item.team.Name,
                    item.team.Speciality,
                   Math.Round(route.DistanceKm, 1),
                   Math.Round(route.DurationMinutes, 0),
                    score,
                    item.isLive));
            }

            return result
                .OrderByDescending(x => x.Score)
                .Take(5)
                .ToList();
        }
    }

  
    
}


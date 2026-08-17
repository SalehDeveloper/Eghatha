using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Common.Services;
using Eghatha.Application.Features.AiAssistant;
using Eghatha.Application.Features.Disasters.Dtos;
using Eghatha.Domain.Shared.ValueObjects;
using ErrorOr;
using MediatR;

namespace Eghatha.Application.Features.Disasters.Queries.CheckDisasterSpam
{
    public class CheckDisasterSpamQueryHandler
       : IRequestHandler<CheckDisasterSpamQuery, ErrorOr<SpamCheckResultDto>>
    {
        private readonly IDisasterRepository _disasterRepo;
        private readonly IRoutingService _routingService;
        private readonly IDuplicateDisasterDetector _detector;

        public CheckDisasterSpamQueryHandler(
            IDisasterRepository disasterRepo,
            IRoutingService routingService,
            IDuplicateDisasterDetector detector)
        {
            _disasterRepo = disasterRepo;
            _routingService = routingService;
            _detector = detector;
        }

        public async Task<ErrorOr<SpamCheckResultDto>> Handle(
            CheckDisasterSpamQuery request, CancellationToken cancellationToken)
        {
            var disaster = await _disasterRepo.GetByIdAsync(request.DisasterId, cancellationToken);
            if (disaster is null)
                return Error.NotFound("Disaster.NotFound", "Disaster not found.");

            var since = DateTimeOffset.UtcNow.AddMinutes(-request.WindowMinutes);

            var candidates = (await _disasterRepo.GetRecentCandidatesByTypeAsync(
                    disaster.Type, since, cancellationToken))
                .Where(c => c.DisasterId != disaster.Id)
                .ToList();

            if (candidates.Count == 0)
                return new SpamCheckResultDto(false, null, 0, "No recent reports of the same type found.");

            var destinations = candidates
                .Select(c => new RouteDestination(c.DisasterId, new GeoLocation(c.Latitude, c.Longitude)))
                .ToList();

            var routes = await _routingService.CalculateAsync(disaster.Location, destinations, cancellationToken);

            var nearbyIds = routes
                .Where(r => r.DistanceKm <= request.RadiusKm)
                .Select(r => r.EntityId)
                .ToHashSet();

            var nearbyCandidates = candidates.Where(c => nearbyIds.Contains(c.DisasterId)).ToList();

            if (nearbyCandidates.Count == 0)
                return new SpamCheckResultDto(false, null, 0, "No same-type reports within the routing radius.");

            var newReport = new NewDisasterReportDto(
                disaster.Id, disaster.Title, disaster.Description,
                disaster.Location.Latitude, disaster.Location.Longitude, disaster.StartTime);

            var result = await _detector.CheckAsync(newReport, nearbyCandidates, cancellationToken);
            if (result.IsError)
                return result.Errors;

            return new SpamCheckResultDto(
                result.Value.IsLikelyDuplicate, result.Value.MatchedDisasterId,
                result.Value.Confidence, result.Value.Reasoning);
        }
    }
}

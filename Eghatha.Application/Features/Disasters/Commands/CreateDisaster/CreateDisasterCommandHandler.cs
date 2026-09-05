using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Services;
using Eghatha.Application.Features.Disasters.Dtos;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Disasters;
using Eghatha.Domain.Shared.ValueObjects;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace Eghatha.Application.Features.Disasters.Commands.CreateDisaster
{
    public class CreateDisasterCommandHandler : IRequestHandler<CreateDisasterCommand, ErrorOr<CreateDisasterDto>>
    {
        private readonly IDisasterRepository _disasterRepository;
        private readonly ITeamRecommendationService _teamRecommendationService;
        private readonly IVolunteerRecommendationService _volunteerRecommendationService;
        private readonly IGeocodingService _geocodingService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HybridCache _hybridCache;


        public CreateDisasterCommandHandler(IDisasterRepository disasterRepository, ITeamRecommendationService teamRecommendationService, IVolunteerRecommendationService volunteerRecommendationService, IGeocodingService geocodingService, IUnitOfWork unitOfWork, HybridCache hybridCache)
        {
            _disasterRepository = disasterRepository;
            _teamRecommendationService = teamRecommendationService;
            _volunteerRecommendationService = volunteerRecommendationService;
            _geocodingService = geocodingService;
            _unitOfWork = unitOfWork;
            _hybridCache = hybridCache;
        }

        public async Task<ErrorOr<CreateDisasterDto>> Handle(
            CreateDisasterCommand request,
            CancellationToken cancellationToken)
        {


            var locationResult = GeoLocation.Create(
                request.Latitude,
                request.Longitude);

            if (locationResult.IsError)
                return locationResult.Errors;

            var loc = await _geocodingService.ResolveAsync(locationResult.Value.Latitude, locationResult.Value.Longitude, cancellationToken);


            var reporterResult = ReporterInfo.Create(
                request.ReporterName,
                request.ReporterPhone,
                request.ReporterNationalId);

            if (reporterResult.IsError)
                return reporterResult.Errors;



            var disasterResult = Disaster.Create(
                Guid.NewGuid(),
                request.DisasterType,
                request.Title,
                request.Description,
                locationResult.Value,
                loc.Province,
                loc.City,
                DateTimeOffset.UtcNow,
                reporterResult.Value,
                request.CustomTypeDescription);

            if (disasterResult.IsError)
                return disasterResult.Errors;

            var disaster = disasterResult.Value;



            await _disasterRepository.AddAsync(
                disaster,
                cancellationToken);



            await _unitOfWork.CompleteAsync(cancellationToken);



            var recommendedTeams =
                await _teamRecommendationService.RecommendAsync(
                    disaster,
                    cancellationToken);



            var recommendedVolunteers =
                await _volunteerRecommendationService.RecommendAsync(
                    disaster,
                    cancellationToken);


            await _hybridCache.RemoveByTagAsync("disasters");
            return new CreateDisasterDto(
                disaster.Id,
                disaster.Status,
                recommendedTeams,
                recommendedVolunteers);
        }
    }
}

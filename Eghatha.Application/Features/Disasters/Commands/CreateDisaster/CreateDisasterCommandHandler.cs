using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Common.Services;
using Eghatha.Application.Features.Disasters.Dtos;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Disasters;
using Eghatha.Domain.Notifications;
using Eghatha.Domain.Shared.ValueObjects;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Commands.CreateDisaster
{
    public class CreateDisasterCommandHandler : IRequestHandler<CreateDisasterCommand, ErrorOr<CreateDisasterDto>>
    {
        private readonly IDisasterRepository _disasterRepository;
        private readonly ITeamRecommendationService _teamRecommendationService;
        private readonly IVolunteerRecommendationService _volunteerRecommendationService;
        private readonly IGeocodingService _geocodingService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateDisasterCommandHandler(IDisasterRepository disasterRepository, ITeamRecommendationService teamRecommendationService, IVolunteerRecommendationService volunteerRecommendationService, IGeocodingService geocodingService, IUnitOfWork unitOfWork)
        {
            _disasterRepository = disasterRepository;
            _teamRecommendationService = teamRecommendationService;
            _volunteerRecommendationService = volunteerRecommendationService;
            _geocodingService = geocodingService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<CreateDisasterDto>> Handle(
            CreateDisasterCommand request,
            CancellationToken cancellationToken)
        {
            
          

            // ---------------------------------------------------
            // Location
            // ---------------------------------------------------

            var locationResult = GeoLocation.Create(
                request.Latitude,
                request.Longitude);

            if (locationResult.IsError)
                return locationResult.Errors;

            var loc = await _geocodingService.ResolveAsync(locationResult.Value.Latitude , locationResult.Value.Longitude, cancellationToken);
            // ---------------------------------------------------
            // Reporter
            // ---------------------------------------------------

            var reporterResult = ReporterInfo.Create(
                request.ReporterName,
                request.ReporterPhone,
                request.ReporterNationalId);

            if (reporterResult.IsError)
                return reporterResult.Errors;

            // ---------------------------------------------------
            // Disaster Creation
            // ---------------------------------------------------

            var disasterResult = Disaster.Create(
                Guid.NewGuid(),
                request.DisasterType,
                request.Title,
                request.Description,
                locationResult.Value,
                loc.Province , 
                loc.City,
                DateTimeOffset.UtcNow,
                reporterResult.Value,
                request.CustomTypeDescription);

            if (disasterResult.IsError)
                return disasterResult.Errors;

            var disaster = disasterResult.Value;

            // ---------------------------------------------------
            // Save Disaster
            // ---------------------------------------------------

            await _disasterRepository.AddAsync(
                disaster,
                cancellationToken);

      

            await _unitOfWork.CompleteAsync( cancellationToken);

            // ---------------------------------------------------
            // Team Recommendation
            // ---------------------------------------------------

            var recommendedTeams =
                await _teamRecommendationService.RecommendAsync(
                    disaster,
                    cancellationToken);

            // ---------------------------------------------------
            // Volunteer Recommendation
            // ---------------------------------------------------

            var recommendedVolunteers =
                await _volunteerRecommendationService.RecommendAsync(
                    disaster,
                    cancellationToken);

            // ---------------------------------------------------
            // Response
            // ---------------------------------------------------

            return new CreateDisasterDto(
                disaster.Id,
                disaster.Status,
                recommendedTeams,
                recommendedVolunteers);
        }
    }
}

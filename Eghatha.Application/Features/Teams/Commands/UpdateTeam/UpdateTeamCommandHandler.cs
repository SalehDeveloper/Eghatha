using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Services;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Shared.ValueObjects;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace Eghatha.Application.Features.Teams.Commands.UpdateTeam
{
    public class UpdateTeamCommandHandler
    : IRequestHandler<UpdateTeamCommand, ErrorOr<Updated>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITeamRepository _teamRepository;
        private readonly HybridCache _hybridCache;
        private readonly IGeocodingService _geocodingService;

        public UpdateTeamCommandHandler(
            IUnitOfWork unitOfWork,
            ITeamRepository teamRepository,
            HybridCache hybridCache,
            IGeocodingService geocodingService)
        {
            _unitOfWork = unitOfWork;
            _teamRepository = teamRepository;
            _hybridCache = hybridCache;
            _geocodingService = geocodingService;
        }

        public async Task<ErrorOr<Updated>> Handle(
            UpdateTeamCommand request,
            CancellationToken cancellationToken)
        {
        
          
            var team = await _teamRepository.GetByIdAsync(request.TeamId);

            if (team is null)
                return ApplicationErrors.TeamNotFound;


            if (request.Longitude is not null && request.Latitude is not null)
            {
               var  locationResult = GeoLocation.Create(
                 request.Latitude.Value,
                 request.Longitude.Value);

                if (locationResult.IsError)
                    return locationResult.Errors;

                var loc = await _geocodingService.ResolveAsync(locationResult.Value.Latitude, locationResult.Value.Longitude, cancellationToken);

                var result = team.Update(request.Name, request.Speciality, locationResult.Value, loc.City, loc.Province);

                if (result.IsError) return result.Errors;

            }

            var res = team.Update(request.Name, request.Speciality , null , null , null );

            if (res.IsError) return res.Errors;


            await _unitOfWork.CompleteAsync(cancellationToken);
            await _hybridCache.RemoveByTagAsync("teams");
            return Result.Updated;
        }
    }



}

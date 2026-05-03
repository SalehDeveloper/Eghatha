using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Services;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Shared.ValueObjects;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace Eghatha.Application.Features.Volunteers.Commands.UpdateLocation
{
    public class UpdateVolunteerLocationCommandHandler
    : IRequestHandler<UpdateVolunteerLocationCommand, ErrorOr<Updated>>
    {
        private readonly IVolunteerRepository _repository;
        private readonly IGeocodingService _geocodingService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HybridCache _hybridCache;


        public UpdateVolunteerLocationCommandHandler(IVolunteerRepository repository, IGeocodingService geocodingService, IUnitOfWork unitOfWork, HybridCache hybridCache)
        {
            _repository = repository;
            _geocodingService = geocodingService;
            _unitOfWork = unitOfWork;
            _hybridCache = hybridCache;
        }

        public async Task<ErrorOr<Updated>> Handle(
            UpdateVolunteerLocationCommand request,
            CancellationToken cancellationToken)
        {
            var volunteer = await _repository.GetByIdAsync(request.VolunteerId, cancellationToken);

            if (volunteer is null)
                return ApplicationErrors.VolunteerNotFound;

            var locationResult = GeoLocation.Create(request.Latitude, request.Longitude);

            if (locationResult.IsError)
                return locationResult.Errors;
            
            var loc = await _geocodingService.ResolveAsync(locationResult.Value.Latitude, locationResult.Value.Longitude, cancellationToken);

            var result = volunteer.UpdateLocation(locationResult.Value , loc.Province , loc.City);

            if (result.IsError)
                return result.Errors;

            await _unitOfWork.CompleteAsync(cancellationToken);
            await _hybridCache.RemoveByTagAsync("volunteers");

            return Result.Updated;
        }
    }
}

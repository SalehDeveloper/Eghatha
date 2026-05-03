using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Services;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Shared.ValueObjects;
using Eghatha.Domain.VolunteerRegisterations;
using Eghatha.Domain.Volunteers;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Volunteers.Commands.CreateVolunteer
{
    public class CreateVolunteerCommandHandler : IRequestHandler<CreateVolunteerCommand, ErrorOr<string>>
    {
        private readonly IIdentityService _identityService;
        private readonly IVolunteerRepository _volunteerRepository;
        private readonly IVolunteerRegisterationRepository _volunteerRegisterationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TimeProvider _timeProvider;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IGeocodingService _geocodingService;
        private readonly HybridCache _hybridCache;


        public CreateVolunteerCommandHandler(
            IIdentityService identityService,
            IVolunteerRepository volunteerRepository,
            IVolunteerRegisterationRepository volunteerRegisterationRepository,
            IUnitOfWork unitOfWork,
            TimeProvider timeProvider,
            ICloudinaryService cloudinaryService,
            IGeocodingService geocodingService,
            HybridCache hybridCache)
        {
            _identityService = identityService;
            _volunteerRepository = volunteerRepository;
            _volunteerRegisterationRepository = volunteerRegisterationRepository;
            _unitOfWork = unitOfWork;
            _timeProvider = timeProvider;
            _cloudinaryService = cloudinaryService;
            _geocodingService = geocodingService;
            _hybridCache = hybridCache;
        }

        public async Task<ErrorOr<string>> Handle(CreateVolunteerCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _identityService.UserExistsAsync(request.Email, cancellationToken);

            if (userExists)
                return ApplicationErrors.UserWithEmailAlreadyExitst;

            //1-upload photo to cloudinary
            var photoPath = await _cloudinaryService.UploadUserPhotoAsync(request.Email, request.photo);
            if (photoPath.IsError) return photoPath.Errors;
            
            //2-create identity user 
            var user = await _identityService.CreatUserAsync(request.FirstName,
                request.LastName,
                request.Email,
                request.PhoneNumber,
                request.Password,
                photoPath.Value,
                Common.Models.UserCreationMode.Regular);
            
            if (user.IsError )return user.Errors;
            await _identityService.AddUserToRoleAsync(user.Value, Domain.Identity.Role.volunteer);

            //3-upload cv to cloudinary  
            var cvPath = await _cloudinaryService.UploadVolunteerCvAsync(request.Email, request.Cv);
            if (cvPath.IsError) return cvPath.Errors;

            //4-create volunteer based to identityUser , 
            var location = GeoLocation.Create(request.Latitude , request.Longitude);
            
            var locationRes = await _geocodingService.ResolveAsync(location.Value.Latitude, location.Value.Longitude, cancellationToken);

            var volunteer = Volunteer.Create(Guid.NewGuid(),
                user.Value,
                VolunteerStatus.UnderReview,
                request.Speciality ,
                location.Value ,
                locationRes.Province,
                locationRes.City,
                request.YearsOfExperience ,
                cvPath.Value);
        
            await _volunteerRepository.AddAsync(volunteer.Value, cancellationToken);
           
            
            //create volunteer registeration , 
            var registeration = VolunteerRegisteration.Create(volunteer.Value.Id, _timeProvider.GetUtcNow());
            if (registeration.IsError) return registeration.Errors;
           
            await _volunteerRegisterationRepository.AddAsync(registeration.Value, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            await _hybridCache.RemoveByTagAsync("volunteers");
            return $"Account created successfully , please check your email";

        }
    }
}

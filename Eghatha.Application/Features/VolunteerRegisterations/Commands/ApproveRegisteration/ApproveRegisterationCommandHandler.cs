using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.VolunteerRegisterations.Commands.ApproveRegisteration
{
    public class ApproveRegisterationCommandHandler : IRequestHandler<ApproveRegisterationCommand, ErrorOr<Updated>>
    {
        private readonly IVolunteerRegisterationRepository _registerationRepository;
        private readonly IVolunteerRepository _volunteerRepositry;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUser _user;
        private readonly TimeProvider _timeProvider;
        private readonly IIdentityService _identityService;
        private readonly HybridCache _hybridCache;

        public ApproveRegisterationCommandHandler(IVolunteerRegisterationRepository registerationRepository, IVolunteerRepository volunteerRepositry, IUnitOfWork unitOfWork, IUser user, TimeProvider timeProvider, IIdentityService identityService, HybridCache hybridCache)
        {
            _registerationRepository = registerationRepository;
            _volunteerRepositry = volunteerRepositry;
            _unitOfWork = unitOfWork;
            _user = user;
            _timeProvider = timeProvider;
            _identityService = identityService;
            _hybridCache = hybridCache;
        }

        public async Task<ErrorOr<Updated>> Handle(ApproveRegisterationCommand request, CancellationToken cancellationToken)
        {
           var registeration = await _registerationRepository.GetByIdAsync(request.RegisterationId, cancellationToken);

           if (registeration is null) return ApplicationErrors.RegisterationNotFound;

           var volunteer = await _volunteerRepositry.GetByIdAsync(registeration.VolunteerId, cancellationToken);

            // var registerationResult =   registeration.Approve(_timeProvider.GetUtcNow(), _user.Id.Value);

            var registerationResult = registeration.Approve(_timeProvider.GetUtcNow(), Guid.Parse("9668180C-06CB-43DD-8CFA-2EF9D617F47E"));
          
            if (registerationResult.IsError) return registerationResult.Errors;

           var activationResult =   await _identityService.ActivateUser(volunteer.UserId);

           if (activationResult.IsError) return activationResult.Errors;

           await _unitOfWork.CompleteAsync(cancellationToken);

            await _hybridCache.RemoveByTagAsync("volunteer-registrations");
            return Result.Updated; 
            
        }
    }
}

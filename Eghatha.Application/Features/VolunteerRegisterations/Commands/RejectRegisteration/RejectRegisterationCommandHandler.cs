using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace Eghatha.Application.Features.VolunteerRegisterations.Commands.RejectRegisteration
{
    public class RejectRegisterationCommandHandler : IRequestHandler<RejectRegisterationCommand, ErrorOr<Updated>>
    {
        private readonly IVolunteerRegisterationRepository _registerationRepository;
        private readonly IVolunteerRepository _volunteerRepositry;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUser _user;
        private readonly TimeProvider _timeProvider;
        private readonly HybridCache _hybridCache;

        public RejectRegisterationCommandHandler(IVolunteerRegisterationRepository registerationRepository, IVolunteerRepository volunteerRepositry, IUnitOfWork unitOfWork, IUser user, TimeProvider timeProvider, HybridCache hybridCache)
        {
            _registerationRepository = registerationRepository;
            _volunteerRepositry = volunteerRepositry;
            _unitOfWork = unitOfWork;
            _user = user;
            _timeProvider = timeProvider;
            _hybridCache = hybridCache;
        }

        public async Task<ErrorOr<Updated>> Handle(RejectRegisterationCommand request, CancellationToken cancellationToken)
        {

            var registeration = await _registerationRepository.GetByIdAsync(request.RegisterationId, cancellationToken);

            if (registeration is null) return ApplicationErrors.RegisterationNotFound;

            var volunteer = await _volunteerRepositry.GetByIdAsync(registeration.VolunteerId, cancellationToken);

           // var registerationResult = registeration.Reject(_timeProvider.GetUtcNow(), _user.Id.Value , request.Reason);

            var registerationResult = registeration.Reject(_timeProvider.GetUtcNow(), Guid.Parse("9668180C-06CB-43DD-8CFA-2EF9D617F47E"), request.Reason);

            if (registerationResult.IsError) return registerationResult.Errors;

            await _unitOfWork.CompleteAsync(cancellationToken);
            await _hybridCache.RemoveByTagAsync("volunteer-registrations");
            return Result.Updated;
        }
    }
}

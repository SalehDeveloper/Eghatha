using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace Eghatha.Application.Features.Volunteers.Commands.UpdateStatus
{
    public class UpdateVolunteerStatusCommandHandler
    : IRequestHandler<UpdateVolunteerStatusCommand, ErrorOr<Updated>>
    {
        private readonly IVolunteerRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HybridCache _hybridCache;


        public UpdateVolunteerStatusCommandHandler(IVolunteerRepository repository, IUnitOfWork unitOfWork, HybridCache hybridCache)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _hybridCache = hybridCache;
        }

        public async Task<ErrorOr<Updated>> Handle(
            UpdateVolunteerStatusCommand request,
            CancellationToken cancellationToken)
        {
            var volunteer = await _repository.GetByIdAsync(request.VolunteerId, cancellationToken);

            if (volunteer is null)
                return ApplicationErrors.VolunteerNotFound;

            var result = volunteer.UpdateStatus(request.Status);

            if (result.IsError)
                return result.Errors;

            await _unitOfWork.CompleteAsync(cancellationToken);
            await _hybridCache.RemoveByTagAsync("volunteers");
            return Result.Updated;
        }
    }
}

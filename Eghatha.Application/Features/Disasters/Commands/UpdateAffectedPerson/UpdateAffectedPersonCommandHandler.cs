using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using ErrorOr;
using MediatR;

namespace Eghatha.Application.Features.Disasters.Commands.UpdateAffectedPerson
{
    public sealed class UpdateAffectedPersonCommandHandler
    : IRequestHandler<UpdateAffectedPersonCommand, ErrorOr<Success>>
    {
        private readonly IDisasterRepository _disasterRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAffectedPersonCommandHandler(
            IDisasterRepository disasterRepository,
            IUnitOfWork unitOfWork)
        {
            _disasterRepository = disasterRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Success>> Handle(
            UpdateAffectedPersonCommand request,
            CancellationToken cancellationToken)
        {
            var disaster = await _disasterRepository
                .GetByIdAsync(request.DisasterId, cancellationToken);

            if (disaster is null)
                return ApplicationErrors.DisasterNotFound;

            var result = disaster.UpdateAffectedPerson(
                request.AffectedPersonId,
                request.Name,
                request.Age,
                request.Phone,
                request.Status,
                request.Notes);

            if (result.IsError)
                return result.Errors;

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success;
        }
    }
}

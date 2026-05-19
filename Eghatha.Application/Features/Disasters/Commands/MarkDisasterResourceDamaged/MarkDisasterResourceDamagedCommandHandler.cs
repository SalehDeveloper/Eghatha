using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using ErrorOr;
using MediatR;

namespace Eghatha.Application.Features.Disasters.Commands.MarkDisasterResourceDamaged
{
    public sealed class MarkDisasterResourceDamagedCommandHandler: IRequestHandler<MarkDisasterResourceDamagedCommand, ErrorOr<Success>>
    {
        private readonly IDisasterRepository _disasterRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MarkDisasterResourceDamagedCommandHandler(
            IDisasterRepository disasterRepository,
            IUnitOfWork unitOfWork)
        {
            _disasterRepository = disasterRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Success>> Handle(
            MarkDisasterResourceDamagedCommand request,
            CancellationToken cancellationToken)
        {
            var disaster = await _disasterRepository
                .GetByIdWithResourcesAsync(
                    request.DisasterId,
                    cancellationToken);

            if (disaster is null)
                return ApplicationErrors.DisasterNotFound;

            var result = disaster.MarkResourceAsDamaged(
                request.DisasterResourceId,
                request.Quantity);

            if (result.IsError)
                return result.Errors;

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success;
        }
    }


}

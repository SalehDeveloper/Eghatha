using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Disasters;
using ErrorOr;
using MediatR;

namespace Eghatha.Application.Features.Disasters.Commands.CloseDisaster
{
    public sealed class CloseDisasterCommandHandler
    : IRequestHandler<CloseDisasterCommand, ErrorOr<Success>>
    {
        private readonly IDisasterRepository _disasterRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CloseDisasterCommandHandler(
            IDisasterRepository disasterRepository,
            IUnitOfWork unitOfWork)
        {
            _disasterRepository = disasterRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Success>> Handle(
            CloseDisasterCommand request,
            CancellationToken cancellationToken)
        {
            var disaster = await _disasterRepository
                .GetByIdAsync(request.DisasterId, cancellationToken);

            if (disaster is null)
                return ApplicationErrors.DisasterNotFound;

            var result = disaster.Close();

            if (result.IsError)
                return result.Errors;

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success;
        }
    }
}

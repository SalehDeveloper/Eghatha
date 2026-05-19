using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Disasters;
using ErrorOr;
using MediatR;

namespace Eghatha.Application.Features.Disasters.Commands.ResolveDisaster
{
    public sealed class ResolveDisasterCommandHandler
    : IRequestHandler<ResolveDisasterCommand, ErrorOr<Success>>
    {
        private readonly IDisasterRepository _disasterRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TimeProvider _timeProvider;


        public ResolveDisasterCommandHandler(
            IDisasterRepository disasterRepository,
            IUnitOfWork unitOfWork,
            TimeProvider timeProvider)
        {
            _disasterRepository = disasterRepository;
            _unitOfWork = unitOfWork;
            _timeProvider = timeProvider;
        }

        public async Task<ErrorOr<Success>> Handle(
            ResolveDisasterCommand request,
            CancellationToken cancellationToken)
        {
            var disaster = await _disasterRepository
                .GetByIdAsync(request.DisasterId, cancellationToken);

            if (disaster is null)
                return ApplicationErrors.DisasterNotFound;

            var result = disaster.Resolve(_timeProvider.GetUtcNow());

            if (result.IsError)
                return result.Errors;

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success;
        }
    }
}

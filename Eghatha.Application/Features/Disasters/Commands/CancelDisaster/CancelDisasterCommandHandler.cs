using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace Eghatha.Application.Features.Disasters.Commands.CancelDisaster
{
    public sealed class CancelDisasterCommandHandler : IRequestHandler<CancelDisasterCommand, ErrorOr<Success>>
    {
        private readonly IDisasterRepository _disasterRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TimeProvider _timeProvider;
        private readonly HybridCache _hybridCache;

        public CancelDisasterCommandHandler(IDisasterRepository disasterRepository, IUnitOfWork unitOfWork, TimeProvider timeProvider, HybridCache hybridCache)
        {
            _disasterRepository = disasterRepository;
            _unitOfWork = unitOfWork;
            _timeProvider = timeProvider;
            _hybridCache = hybridCache;
        }

        public async Task<ErrorOr<Success>> Handle(CancelDisasterCommand request, CancellationToken cancellationToken)
        {
            var disaster = await _disasterRepository
               .GetByIdAsync(request.DisasterId, cancellationToken);

            if (disaster is null)
                return ApplicationErrors.DisasterNotFound;

            var result = disaster.Cancel(_timeProvider.GetUtcNow());

            if (result.IsError)
                return result.Errors;

            await _unitOfWork.CompleteAsync(cancellationToken);

            await _hybridCache.RemoveByTagAsync("disasters");
            return Result.Success;

        }
    }
}

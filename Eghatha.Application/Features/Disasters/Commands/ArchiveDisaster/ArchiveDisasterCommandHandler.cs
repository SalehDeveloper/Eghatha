using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace Eghatha.Application.Features.Disasters.Commands.ArchiveDisaster
{
    public sealed class ArchiveDisasterCommandHandler :
        IRequestHandler<ArchiveDisasterCommand, ErrorOr<Success>>
    {
        private readonly IDisasterRepository _disasterRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HybridCache _hybridCache;

        public ArchiveDisasterCommandHandler(IDisasterRepository disasterRepository, IUnitOfWork unitOfWork, HybridCache hybridCache)
        {
            _disasterRepository = disasterRepository;
            _unitOfWork = unitOfWork;
            _hybridCache = hybridCache;
        }

        public async  Task<ErrorOr<Success>> Handle(ArchiveDisasterCommand request, CancellationToken cancellationToken)
        {
            var disaster = await _disasterRepository
                 .GetByIdAsync(request.DisasterId, cancellationToken);

            if (disaster is null)
                return ApplicationErrors.DisasterNotFound;

            var result = disaster.Archive();

            if (result.IsError)
                return result.Errors;

            await _unitOfWork.CompleteAsync(cancellationToken);
            await _hybridCache.RemoveByTagAsync("disasters");
            return Result.Success;
        }
    }
}
using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Disasters.AffectedPersons;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace Eghatha.Application.Features.Disasters.Commands.AddAffectedPersons
{
    public sealed class AddAffectedPersonsCommandHandler
    : IRequestHandler<AddAffectedPersonsCommand, ErrorOr<Success>>
    {
        private readonly IDisasterRepository _disasterRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HybridCache _hybridCache;

        public AddAffectedPersonsCommandHandler(
            IDisasterRepository disasterRepository,
            IUnitOfWork unitOfWork,
            HybridCache hybridCache)
        {
            _disasterRepository = disasterRepository;
            _unitOfWork = unitOfWork;
            _hybridCache = hybridCache;
        }

        public async Task<ErrorOr<Success>> Handle(
            AddAffectedPersonsCommand request,
            CancellationToken cancellationToken)
        {
            var disaster = await _disasterRepository
                .GetByIdWithAffectedPersonsAsync(request.DisasterId, cancellationToken);

            if (disaster is null)
                return ApplicationErrors.DisasterNotFound;

            var data = request.Persons
                .Select(p => (
                    p.Name,
                    p.Age,
                    p.Phone,
                   HealthStatus.FromName( p.Status , true),
                    p.Notes));

            var result = disaster.AddAffectedPersons(data);

            if (result.IsError)
                return result.Errors;

            await _disasterRepository.AddAffectedPersonsAsync(result.Value, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);


            await _hybridCache.RemoveByTagAsync("disasters");

            return Result.Success;
        }
    }
}

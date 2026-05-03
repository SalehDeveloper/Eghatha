using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace Eghatha.Application.Features.Volunteers.Commands.IncreaseVolunteerEquipment
{
    public sealed class IncreaseVolunteerEquipmentQuantityCommandHandler
    : IRequestHandler<IncreaseVolunteerEquipmentQuantityCommand, ErrorOr<Updated>>
    {
        private readonly IVolunteerRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HybridCache _cache;

        public IncreaseVolunteerEquipmentQuantityCommandHandler(
            IVolunteerRepository repo,
            IUnitOfWork unitOfWork,
            HybridCache cache)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<ErrorOr<Updated>> Handle(
            IncreaseVolunteerEquipmentQuantityCommand request,
            CancellationToken cancellationToken)
        {
            var volunteer = await _repo.GetByIdWithEquipmentsAsync(request.VolunteerId, cancellationToken);

            if (volunteer is null)
                return ApplicationErrors.VolunteerNotFound;

            var result = volunteer.IncreaseEquipmentQuantity(request.EquipmentId, request.Quantity);

            if (result.IsError)
                return result.Errors;

            await _unitOfWork.CompleteAsync(cancellationToken);

            await _cache.RemoveByTagAsync("volunteers");

            return Result.Updated;
        }
    }
}

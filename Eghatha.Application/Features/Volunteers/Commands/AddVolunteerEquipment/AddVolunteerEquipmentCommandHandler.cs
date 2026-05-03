using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using ErrorOr;
using MediatR;

namespace Eghatha.Application.Features.Volunteers.Commands.AddVolunteerEquipment
{
    public sealed class AddVolunteerEquipmentCommandHandler
    : IRequestHandler<AddVolunteerEquipmentCommand, ErrorOr<Updated>>
    {
        private readonly IVolunteerRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public AddVolunteerEquipmentCommandHandler(
            IVolunteerRepository repo,
            IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Updated>> Handle(
            AddVolunteerEquipmentCommand request,
            CancellationToken cancellationToken)
        {
            var volunteer = await _repo.GetByIdWithEquipmentsAsync(request.VolunteerId, cancellationToken);

            if (volunteer is null)
                return ApplicationErrors.VolunteerNotFound;

            var result = volunteer.AddEquipment(
                request.Name,
                request.Category,
                request.Quantity);

            if (result.IsError)
                return result.Errors;

            await _repo.AddEquipmentAsync(result.Value , cancellationToken);
        
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Updated;
        }
    }
}

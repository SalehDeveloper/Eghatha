using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using ErrorOr;
using MediatR;

namespace Eghatha.Application.Features.Disasters.Commands.AssignVolunteers
{
    public sealed class AssignVolunteersCommandHandler : IRequestHandler<AssignVolunteersCommand, ErrorOr<Success>>
    {
        private readonly IDisasterRepository _disasterRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignVolunteersCommandHandler(IDisasterRepository disasterRepository, IUnitOfWork unitOfWork)
        {
            _disasterRepository = disasterRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Success>> Handle(AssignVolunteersCommand request, CancellationToken cancellationToken)
        {
            var disaster = await _disasterRepository.GetByIdWithVolunteersAsync(request.DisasterId, cancellationToken);

            if (disaster == null)
                return ApplicationErrors.DisasterNotFound;

           var res =  disaster.AssignVolunteers(request.VolunteerIds);

            if (res.IsError)
                return res.Errors;

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success;
        }
    }
}

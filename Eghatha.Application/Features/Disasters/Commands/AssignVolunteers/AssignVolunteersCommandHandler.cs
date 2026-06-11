using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Disasters;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace Eghatha.Application.Features.Disasters.Commands.AssignVolunteers
{
    public sealed class AssignVolunteersCommandHandler : IRequestHandler<AssignVolunteersCommand, ErrorOr<Success>>
    {
        private readonly IDisasterRepository _disasterRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HybridCache _hybridCache;

        public AssignVolunteersCommandHandler(IDisasterRepository disasterRepository, IUnitOfWork unitOfWork, HybridCache hybridCache)
        {
            _disasterRepository = disasterRepository;
            _unitOfWork = unitOfWork;
            _hybridCache = hybridCache;
        }

        public async Task<ErrorOr<Success>> Handle(AssignVolunteersCommand request, CancellationToken cancellationToken)
        {
            var disaster = await _disasterRepository.GetByIdWithVolunteersAsync(request.DisasterId, cancellationToken);

            if (disaster == null)
                return ApplicationErrors.DisasterNotFound;

           var res =  disaster.AssignVolunteers(request.VolunteerIds);

            if (res.IsError)
                return res.Errors;

            var newVolunteers = res.Value;

            if (newVolunteers.Count > 0)
            {
                await _disasterRepository.AddVolunteersAsync(newVolunteers);
            }
            else
            {
                return DisasterErrors.FailedToAssign;
            }

            await _unitOfWork.CompleteAsync(cancellationToken);
            await _hybridCache.RemoveByTagAsync("disasters");
            return Result.Success;
        }
    }
}

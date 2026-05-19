using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Commands.ConsumeResource
{
    public class ConsumeDisasterResourceCommandHandler : IRequestHandler<ConsumeDisasterResourceCommand, ErrorOr<Success>>
    {
        private readonly IDisasterRepository _disasterRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ConsumeDisasterResourceCommandHandler(IDisasterRepository disasterRepository, IUnitOfWork unitOfWork)
        {
            _disasterRepository = disasterRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Success>> Handle(ConsumeDisasterResourceCommand request, CancellationToken cancellationToken)
        {
            var disaster = await _disasterRepository.GetByIdWithResourcesAsync(request.DisasterId, cancellationToken);
            
            if (disaster is null) return ApplicationErrors.DisasterNotFound;

            var result = disaster.ConsumeResource(request.DisasterResourceId, request.quantity);

            if (result.IsError) return result.Errors;

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success;
        }
    }
}

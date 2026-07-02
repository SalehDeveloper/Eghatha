using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Disasters;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace Eghatha.Application.Features.Disasters.Commands.ReturnResource
{
    public sealed class   ReturnDisasterResourceCommandHandler:IRequestHandler<ReturnDisasterResourceCommand, ErrorOr<Success>>
    {
        private readonly IDisasterRepository _disasterRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HybridCache _hybridCache;


        public ReturnDisasterResourceCommandHandler(IDisasterRepository disasterRepository, ITeamRepository teamRepository, IUnitOfWork unitOfWork, HybridCache hybridCache)
        {
            _disasterRepository = disasterRepository;
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
            _hybridCache = hybridCache;
        }

        public async Task<ErrorOr<Success>> Handle(ReturnDisasterResourceCommand request, CancellationToken cancellationToken)
        {
            var disaster = await _disasterRepository.GetByIdWithResourcesAsync(request.DisasterId, cancellationToken);
            
            if (disaster == null)
                return ApplicationErrors.DisasterNotFound;

            var disasterResource = disaster.Resources.FirstOrDefault(x => x.Id == request.DisasterResourceId);

            if (disasterResource is null) return ApplicationErrors.DisasterResourceNotFound;

            var team = await _teamRepository.GetTeamByIdWithResourcesAsync(disasterResource.TeamId, cancellationToken);

            var returnResult = disaster.ReturnResource(request.DisasterResourceId, request.Quantity);

            if (returnResult.IsError)
                return returnResult.Errors;

            var restoreResult = team.ReturnResource(disasterResource.ResourceId, request.Quantity);
           
            if (restoreResult.IsError) 
                return restoreResult.Errors; 

            await _unitOfWork.CompleteAsync(cancellationToken);

            await _hybridCache.RemoveByTagAsync("disasters");
            return Result.Success;
        }
    }


}

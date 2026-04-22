using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace Eghatha.Application.Features.Teams.Commands.UpdateTeam
{
    public class UpdateTeamCommandHandler
    : IRequestHandler<UpdateTeamCommand, ErrorOr<Updated>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITeamRepository _teamRepository;
        private readonly HybridCache _hybridCache;

        public UpdateTeamCommandHandler(
            IUnitOfWork unitOfWork,
            ITeamRepository teamRepository,
            HybridCache hybridCache)
        {
            _unitOfWork = unitOfWork;
            _teamRepository = teamRepository;
            _hybridCache = hybridCache;
        }

        public async Task<ErrorOr<Updated>> Handle(
            UpdateTeamCommand request,
            CancellationToken cancellationToken)
        {
          
            var team = await _teamRepository.GetByIdAsync(request.TeamId);

            if (team is null)
                return ApplicationErrors.TeamNotFound;

       
            var result = team.Update(
                request.Name,
                request.Speciality,
                request.Province,
                request.City
            );

            if (result.IsError)
                return result.Errors;

           
            await _unitOfWork.CompleteAsync(cancellationToken);
            await _hybridCache.RemoveByTagAsync("teams");
            return Result.Updated;
        }
    }



}

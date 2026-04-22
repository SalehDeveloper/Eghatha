using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace Eghatha.Application.Features.Teams.Commands.UpdateTeamStatus
{
    public class UpdateTeamStatusCommandHandler
    : IRequestHandler<UpdateTeamStatusCommand, ErrorOr<Updated>>
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HybridCache _hybridCache;

        public UpdateTeamStatusCommandHandler(
            ITeamRepository teamRepository,
            IUnitOfWork unitOfWork,
            HybridCache hybridCache)
        {
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
            _hybridCache = hybridCache;
        }

        public async Task<ErrorOr<Updated>> Handle(
            UpdateTeamStatusCommand request,
            CancellationToken cancellationToken)
        {
            var team = await _teamRepository.GetTeamByIdWithMembersAsync(request.Id , cancellationToken);

            if (team is null)
                return ApplicationErrors.TeamNotFound;

            var result = team.UpdateStatus(request.Status);

            if (result.IsError)
                return result.Errors;

            await _unitOfWork.CompleteAsync(cancellationToken);
            await _hybridCache.RemoveByTagAsync("teams");
            return Result.Updated;
        }
    }
}

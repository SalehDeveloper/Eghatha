using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Teams.Resources;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace Eghatha.Application.Features.Teams.Commands.IncreaseTeamResource
{
    public sealed class IncreaseTeamResourceCommandHandler
    : IRequestHandler<IncreaseTeamResourceCommand, ErrorOr<Updated>>
    {
        private readonly ITeamRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HybridCache _hybridCache;

        public IncreaseTeamResourceCommandHandler(ITeamRepository repo, IUnitOfWork unitOfWork, HybridCache hybridCache)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
            _hybridCache = hybridCache;
        }

        public async Task<ErrorOr<Updated>> Handle(
            IncreaseTeamResourceCommand request,
            CancellationToken cancellationToken)
        {
            var team = await _repo.GetTeamByIdWithResourcesAsync(request.TeamId , cancellationToken);

            if (team is null)
                return ApplicationErrors.TeamNotFound;

       

            var result = team.IncreaseResourceQuantity(request.ResourceId, request.Quantity);

            if (result.IsError)
                return result.Errors;

            await _unitOfWork.CompleteAsync(cancellationToken);

            await _hybridCache.RemoveByTagAsync("teams");

            return Result.Updated;
        }
    }
}

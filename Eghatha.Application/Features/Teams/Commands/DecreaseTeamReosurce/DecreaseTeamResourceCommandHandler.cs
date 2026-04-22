using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace Eghatha.Application.Features.Teams.Commands.DecreaseTeamReosurce
{
    public sealed class DecreaseTeamResourceCommandHandler
    : IRequestHandler<DecreaseTeamResourceCommand, ErrorOr<Updated>>
    {
        private readonly ITeamRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HybridCache _hybridCache;

        public DecreaseTeamResourceCommandHandler(ITeamRepository repo, IUnitOfWork unitOfWork, HybridCache hybridCache)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
            _hybridCache = hybridCache;
        }

        public async Task<ErrorOr<Updated>> Handle(
            DecreaseTeamResourceCommand request,
            CancellationToken cancellationToken)
        {
            var team = await _repo.GetTeamByIdWithResourcesAsync(request.TeamId,cancellationToken);

            if (team is null)
                return ApplicationErrors.TeamNotFound;
             
            var res = team.DecreaseResourceQuantity(request.ResourceId , request.Quantity); 

            if (res.IsError)
                return res.Errors;

            await _unitOfWork.CompleteAsync(cancellationToken);
            await _hybridCache.RemoveByTagAsync("teams");

            return Result.Updated;
        }
    }
}

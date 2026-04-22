using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Commands.AddTeamResource
{
    public class AddTeamResourceCommandHandler : IRequestHandler<AddTeamResourceCommand, ErrorOr<Updated>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITeamRepository _teamRepository;
        private readonly HybridCache _hybridCache;

        public AddTeamResourceCommandHandler(IUnitOfWork unitOfWork, ITeamRepository teamRepository, HybridCache hybridCache)
        {
            _unitOfWork = unitOfWork;
            _teamRepository = teamRepository;
            _hybridCache = hybridCache;
        }

        public async Task<ErrorOr<Updated>> Handle(AddTeamResourceCommand request, CancellationToken cancellationToken)
        {
            var team = await _teamRepository.GetTeamByIdWithResourcesAsync(request.TeamId, cancellationToken);

            if (team == null) return ApplicationErrors.TeamNotFound;

            var res = team.AddResource(request.Quantity, request.Type);

            if (res.IsError) return res.Errors;

            await _teamRepository.AddTeamResourceAsync(res.Value, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await _hybridCache.RemoveByTagAsync("teams");
            return Result.Updated;

        }
    }
}

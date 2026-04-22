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

namespace Eghatha.Application.Features.Teams.Commands.ChangeTeamLeader
{
    public class ChangeTeamLeaderCommandHandler : IRequestHandler<ChangeTeamLeaderCommand, ErrorOr<Updated>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITeamRepository _teamRepository;
        private readonly HybridCache _hybridCache;

        public ChangeTeamLeaderCommandHandler(IUnitOfWork unitOfWork, ITeamRepository teamRepository, HybridCache hybridCache)
        {
            _unitOfWork = unitOfWork;
            _teamRepository = teamRepository;
            _hybridCache = hybridCache;
        }

        public async Task<ErrorOr<Updated>> Handle(ChangeTeamLeaderCommand request, CancellationToken cancellationToken)
        {
            var team = await _teamRepository.GetTeamByIdWithMembersAsync(request.TeamId, cancellationToken);

            if (team == null) return ApplicationErrors.TeamNotFound;

            var result = team.ChangeLeader(request.MemberId);

            if (result.IsError)
                return result.Errors;
            

            await _unitOfWork.CompleteAsync(cancellationToken);
            await _hybridCache.RemoveByTagAsync("teams");
            return Result.Updated;
        }
    }
}

using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Teams;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace Eghatha.Application.Features.Disasters.Commands.AssignTeams
{
    public class AssignTeamsCommandHandler : IRequestHandler<AssignTeamsCommand, ErrorOr<Success>>
    {
        private readonly IDisasterRepository _disasterRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HybridCache _hybridCache;

        public AssignTeamsCommandHandler(ITeamRepository teamRepository, IUnitOfWork unitOfWork, IDisasterRepository disasterRepository, HybridCache hybridCache)
        {
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
            _disasterRepository = disasterRepository;
            _hybridCache = hybridCache;
        }

        public async Task<ErrorOr<Success>> Handle(AssignTeamsCommand request, CancellationToken cancellationToken)
        {
            var disaster = await _disasterRepository.GetByIdWithTeamsAsync(request.DisasterId, cancellationToken);

            if (disaster == null) return ApplicationErrors.DisasterNotFound;

            var teams = await _teamRepository.GetTeamsByIdsAsync(request.TeamIds, cancellationToken);

            if (teams.Count != request.TeamIds.Count) return ApplicationErrors.TeamNotFound;

            foreach (var team in teams)
            {
                if (team.Status != TeamStatus.Active && team.Status != TeamStatus.Returning)
                {
                    return ApplicationErrors.TeamNotAvailable;
                }

                var assignResult = disaster.AssignTeam(team.Id);

                if (assignResult.IsError)
                    return assignResult.Errors;

                team.UpdateStatus(TeamStatus.OnMission);


            }

            disaster.StartResponse();

            await _unitOfWork.CompleteAsync(cancellationToken);

            await _hybridCache.RemoveByTagAsync("disasters");
            await _hybridCache.RemoveByTagAsync("teams");
            return Result.Success;
        }
    }
}

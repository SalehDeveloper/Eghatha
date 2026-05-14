using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Disasters;
using Eghatha.Domain.Teams;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Commands.AssignTeams
{
    public class AssignTeamsCommandHandler : IRequestHandler<AssignTeamsCommand, ErrorOr<Success>>
    {
        private readonly IDisasterRepository _disasterRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignTeamsCommandHandler(ITeamRepository teamRepository, IUnitOfWork unitOfWork, IDisasterRepository disasterRepository)
        {
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
            _disasterRepository = disasterRepository;
        }

        public async Task<ErrorOr<Success>> Handle(AssignTeamsCommand request, CancellationToken cancellationToken)
        {
            var disaster = await _disasterRepository.GetByIdWithTeamsAsync(request.DisasterId, cancellationToken);

            if (disaster == null) return ApplicationErrors.DisasterNotFound;
           
            var teams = await _teamRepository.GetTeamsByIdsAsync(request.TeamIds, cancellationToken);

            if (teams.Count != request.TeamIds.Count) return ApplicationErrors.TeamNotFound;

            foreach(var team in teams )
            {
                if (team.Status != TeamStatus.Active && team.Status != TeamStatus.Returning)
                {
                    return Error.Conflict(
                    code: "Team.NotAvailable",
                    description: $"Team '{team.Name}' is not available");
                }

                var assignResult = disaster.AssignTeam(team.Id);

                if (assignResult.IsError)
                    return assignResult.Errors;

                team.UpdateStatus(TeamStatus.OnMission);
                    
                
            }

            disaster.StartResponse();

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success;
        }
    }
}

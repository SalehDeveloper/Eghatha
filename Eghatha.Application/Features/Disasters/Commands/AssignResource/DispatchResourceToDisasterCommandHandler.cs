using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Teams.Resources;
using ErrorOr;
using MediatR;

namespace Eghatha.Application.Features.Disasters.Commands.AssignResource
{
    public class DispatchResourceToDisasterCommandHandler : IRequestHandler<DispatchResourceToDisasterCommand, ErrorOr<Success>>
    {
        private readonly IDisasterRepository _disasterRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DispatchResourceToDisasterCommandHandler(IDisasterRepository disasterRepository, ITeamRepository teamRepository, IUnitOfWork unitOfWork)
        {
            _disasterRepository = disasterRepository;
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Success>> Handle(DispatchResourceToDisasterCommand request, CancellationToken cancellationToken)
        {
            // get disaster with resources and teams 
            var disaster = await _disasterRepository.GetByIdWithTeamsAndResources(request.DisasterId, cancellationToken);
            if (disaster is null) return ApplicationErrors.DisasterNotFound;

            // check if the team has the resource

            var team = await _teamRepository.GetTeamByIdWithResourcesAsync(request.TeamId, cancellationToken);
            if (team is null) return ApplicationErrors.TeamNotFound;

 
            // consume from team inventory
            var consumeResult = team.DeductResource(
                request.ResourceId,
                request.Quantity);

            if (consumeResult.IsError)
                return consumeResult.Errors;

            // assign to disaster
            var assignResult = disaster.DispatchResource(
                request.ResourceId,
                request.TeamId,
                consumeResult.Value.Type,
                request.Quantity,
                DateTimeOffset.UtcNow,
                request.Notes);

            if (assignResult.IsError)
                return assignResult.Errors;

            await _disasterRepository.AddResourceAsync(assignResult.Value , cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success;

        }
    }
}

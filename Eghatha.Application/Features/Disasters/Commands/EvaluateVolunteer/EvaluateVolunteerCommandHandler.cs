using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Disasters.DisasterVolunteers;
using ErrorOr;
using MediatR;

namespace Eghatha.Application.Features.Disasters.Commands.EvaluateVolunteer
{
    public sealed class EvaluateVolunteerCommandHandler
    : IRequestHandler<EvaluateVolunteerCommand, ErrorOr<Success>>
    {
        private readonly IDisasterRepository _disasterRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUser _currentUserProvider;
        private readonly TimeProvider _timeProvider;

        public EvaluateVolunteerCommandHandler(
            IDisasterRepository disasterRepository,
            IUnitOfWork unitOfWork,
            IUser currentUserProvider,
            TimeProvider timeProvider)
        {
            _disasterRepository = disasterRepository;
            _unitOfWork = unitOfWork;
            _currentUserProvider = currentUserProvider;
            _timeProvider = timeProvider;
        }

        public async Task<ErrorOr<Success>> Handle(
            EvaluateVolunteerCommand request,
            CancellationToken cancellationToken)
        {
            var disaster = await _disasterRepository
                .GetByIdWithVolunteersAsync(
                    request.DisasterId,
                    cancellationToken);

            if (disaster is null)
                return ApplicationErrors.DisasterNotFound;

            var evaluationResult = EvaluationScores.Create(
                request.CommitmentScore,
                request.SkillScore,
                request.SafetyScore,
                request.TeamWorkScore,
                request.InitiativeScore);

            if (evaluationResult.IsError)
                return evaluationResult.Errors;

            var leaderId = _currentUserProvider.Id;

            var result = disaster.EvaluateVolunteer(
                request.VolunteerId,
                evaluationResult.Value,
                request.Notes,
                _timeProvider.GetUtcNow(),
                leaderId.Value);

            if (result.IsError)
                return result.Errors;

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success;
        }
    }
}

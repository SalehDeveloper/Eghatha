using ErrorOr;
using MediatR;


namespace Eghatha.Application.Features.Disasters.Commands.EvaluateVolunteer
{
    public sealed record EvaluateVolunteerCommand(
     Guid DisasterId,
     Guid VolunteerId,
     int CommitmentScore,
     int SkillScore,
     int SafetyScore,
     int TeamWorkScore,
     int InitiativeScore,
     string? Notes)
     : IRequest<ErrorOr<Success>>;
}

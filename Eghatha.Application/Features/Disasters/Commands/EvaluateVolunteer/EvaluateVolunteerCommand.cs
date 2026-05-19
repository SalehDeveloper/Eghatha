using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

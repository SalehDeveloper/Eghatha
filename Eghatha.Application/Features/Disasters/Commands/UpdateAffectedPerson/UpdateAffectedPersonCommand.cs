using Eghatha.Domain.Disasters;
using Eghatha.Domain.Disasters.AffectedPersons;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Commands.UpdateAffectedPerson
{
    public sealed record UpdateAffectedPersonCommand(
     Guid DisasterId,
     Guid AffectedPersonId,
     string Name,
     int Age,
     string Phone,
     HealthStatus Status,
     string? Notes
 ) : IRequest<ErrorOr<Success>>;
}

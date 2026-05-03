using Eghatha.Domain.Shared.Errors;
using Eghatha.Domain.Volunteers;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Volunteers.Commands.UpdateStatus
{
    public record UpdateVolunteerStatusCommand(
     Guid VolunteerId,
     VolunteerStatus Status
 ) : IRequest<ErrorOr<Updated>>;
}

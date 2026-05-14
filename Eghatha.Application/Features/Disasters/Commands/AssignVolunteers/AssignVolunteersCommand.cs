using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Commands.AssignVolunteers
{
    public sealed record AssignVolunteersCommand(Guid DisasterId , List<Guid> VolunteerIds) : IRequest<ErrorOr<Success>>;
}

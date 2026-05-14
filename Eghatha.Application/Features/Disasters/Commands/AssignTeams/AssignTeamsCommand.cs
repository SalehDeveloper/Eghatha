using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Commands.AssignTeams
{
    public sealed record AssignTeamsCommand(Guid DisasterId, List<Guid> TeamIds) : IRequest<ErrorOr<Success>>;
    
    
}

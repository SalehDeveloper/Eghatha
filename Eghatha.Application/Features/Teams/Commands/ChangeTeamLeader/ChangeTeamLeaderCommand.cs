using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Commands.ChangeTeamLeader
{
    public record ChangeTeamLeaderCommand(Guid TeamId, Guid MemberId) : IRequest<ErrorOr<Updated>>;
    
    
}

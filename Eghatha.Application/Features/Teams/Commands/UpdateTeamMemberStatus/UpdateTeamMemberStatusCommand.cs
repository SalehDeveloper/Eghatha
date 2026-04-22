using Eghatha.Domain.Teams.TeamMembers;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Commands.DeActivateTeamMember
{
    public record UpdateTeamMemberStatusCommand(Guid TeamId, Guid MemeberId , TeamMemberStatus Status) : IRequest<ErrorOr<Updated>>;
    
    
}

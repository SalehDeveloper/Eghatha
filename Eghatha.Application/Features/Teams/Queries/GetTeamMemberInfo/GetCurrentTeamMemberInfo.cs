using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Queries.GetTeamMemberInfo
{
    public sealed record GetCurrentTeamMemberInfo : IRequest<TeamMemberInfo>;

    public sealed record TeamMemberInfo(
        Guid TeamId,
        bool IsLeader
    );
}
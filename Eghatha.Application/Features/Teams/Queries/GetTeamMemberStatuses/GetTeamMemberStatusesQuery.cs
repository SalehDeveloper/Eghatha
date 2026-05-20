using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Queries.GetTeamMemberStatuses
{
    public sealed record GetTeamMemberStatusesQuery
    : IRequest<IReadOnlyList<TeamMemberStatusResponse>>;

    public sealed record TeamMemberStatusResponse(int Value, string Name);
}

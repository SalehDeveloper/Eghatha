using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Queries.GetTeamStatuses
{
    public sealed record GetTeamStatusesQuery
    : IRequest<IReadOnlyList<TeamStatusResponse>>;

    public sealed record TeamStatusResponse(int Value, string Name);
}

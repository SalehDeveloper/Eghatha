using Eghatha.Application.Features.Teams.Queries.GetTeamDisasters;
using Eghatha.Domain.Disasters;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Queries.GetCurrentTeamDisaster
{
    public sealed record GetCurrentTeamDisasterQuery(Guid TeamId) : IRequest<ErrorOr<TeamDisastersDto>>;



}

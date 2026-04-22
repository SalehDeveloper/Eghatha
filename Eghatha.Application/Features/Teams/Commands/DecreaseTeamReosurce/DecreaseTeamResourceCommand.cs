using Eghatha.Domain.Teams;
using Eghatha.Domain.Teams.Resources;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Commands.DecreaseTeamReosurce
{
    public sealed record DecreaseTeamResourceCommand(
     Guid TeamId,
     Guid ResourceId,
     int Quantity
 ) : IRequest<ErrorOr<Updated>>;
}

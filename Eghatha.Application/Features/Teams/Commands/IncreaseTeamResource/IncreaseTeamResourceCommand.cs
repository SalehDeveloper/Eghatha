using Eghatha.Domain.Teams;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Commands.IncreaseTeamResource
{
    public sealed record IncreaseTeamResourceCommand(
     Guid TeamId,
     Guid ResourceId,
     int Quantity
 ) : IRequest<ErrorOr<Updated>>;
}

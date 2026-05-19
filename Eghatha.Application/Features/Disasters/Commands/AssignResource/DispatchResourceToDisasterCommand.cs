using Eghatha.Domain.Teams.Resources;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Commands.AssignResource
{
    public sealed record DispatchResourceToDisasterCommand(Guid DisasterId, Guid ResourceId, Guid TeamId, int Quantity, string? Notes)
        : IRequest<ErrorOr<Success>>;
}

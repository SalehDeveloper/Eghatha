using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Commands.MarkDisasterResourceDamaged
{
    public sealed record MarkDisasterResourceDamagedCommand(Guid DisasterId,
    Guid DisasterResourceId,
    int Quantity) : IRequest<ErrorOr<Success>>;


}

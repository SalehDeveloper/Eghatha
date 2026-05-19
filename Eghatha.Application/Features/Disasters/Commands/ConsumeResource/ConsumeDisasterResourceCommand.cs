using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Commands.ConsumeResource
{
    public sealed record ConsumeDisasterResourceCommand(Guid DisasterId, Guid DisasterResourceId, int quantity)
        : IRequest<ErrorOr<Success>>;
    
    
}

using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Commands.CloseDisaster
{
    public sealed record CloseDisasterCommand(Guid DisasterId) 
        : IRequest<ErrorOr<Success>>;
}

using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Commands.ResolveDisaster
{
    public sealed record ResolveDisasterCommand(Guid DisasterId) : IRequest<ErrorOr<Success>>;
}

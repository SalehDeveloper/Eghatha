using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.VolunteerRegisterations.Commands.RejectRegisteration
{
    public record RejectRegisterationCommand(Guid RegisterationId , string Reason) : IRequest<ErrorOr<Updated>>;
}

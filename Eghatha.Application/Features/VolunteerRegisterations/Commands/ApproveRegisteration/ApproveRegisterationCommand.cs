using Eghatha.Application.Features.VolunteerRegisterations.Commands.RejectRegisteration;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.VolunteerRegisterations.Commands.ApproveRegisteration
{
    public record ApproveRegisterationCommand(Guid RegisterationId) : IRequest<ErrorOr<Updated>>;

}

using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Authentication.Commands.RequestPasswordReset
{
    public record RequestPasswordResetCommand(string Email) : IRequest<ErrorOr<string>>;
    
    
}

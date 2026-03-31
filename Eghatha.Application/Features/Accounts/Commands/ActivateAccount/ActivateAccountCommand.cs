using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Accounts.Commands.ActivateAccount
{
    public record ActivateAccountCommand(Guid Id) : IRequest<ErrorOr<Success>>;
    
    
}

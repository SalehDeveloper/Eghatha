using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Accounts.Commands.DeActivateAccount
{
    public record DeActivateAccountCommand(Guid Id) : IRequest<ErrorOr<Success>>;



}

using Eghatha.Application.Common.Authentication;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Authentication.Commands.Login
{
    public sealed  record LoginCommand(string Email , string Password):IRequest<ErrorOr<AppUserDto>>;
}

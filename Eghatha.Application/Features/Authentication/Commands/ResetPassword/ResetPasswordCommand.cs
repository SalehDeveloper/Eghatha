using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Authentication.Commands.ResetPassword
{
    public record class ResetPasswordCommand(string Email , string Otp , string NewPassword) : IRequest<ErrorOr<string>>;
}

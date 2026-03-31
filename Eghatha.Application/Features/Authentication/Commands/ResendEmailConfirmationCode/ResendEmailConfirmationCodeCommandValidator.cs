using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Authentication.Commands.ResendEmailConfirmationCode
{
    public class ResendEmailConfirmationCodeCommandValidator :AbstractValidator<ResendEmailConfirmationCodeCommand>
    {
        public ResendEmailConfirmationCodeCommandValidator()
        {
            RuleFor(x => x.Email)
          .NotEmpty().WithMessage("Email is required.")
          .EmailAddress().WithMessage("Email must be valid.");
        }
    }
}

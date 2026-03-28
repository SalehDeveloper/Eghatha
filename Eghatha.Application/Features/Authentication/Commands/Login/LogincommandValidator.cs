using FluentValidation;

namespace Eghatha.Application.Features.Authentication.Commands.Login
{
    public class LogincommandValidator : AbstractValidator<LoginCommand>
    {
        public LogincommandValidator()
        {
            RuleFor(x => x.Email)
             .NotEmpty().WithMessage("Email is required.")
             .EmailAddress().WithMessage("Email must be valid.");

            RuleFor(x => x.Password)
             .NotEmpty().WithMessage("Password is required.");

        }
    }
}

using FluentValidation;

namespace Eghatha.Application.Features.Accounts.Commands.DeActivateAccount
{
    public class DeActivateAccountCommandValidator : AbstractValidator<DeActivateAccountCommand>
    {
        public DeActivateAccountCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty()
               .NotNull()
               .WithMessage("Id is required");
        }
    }



}

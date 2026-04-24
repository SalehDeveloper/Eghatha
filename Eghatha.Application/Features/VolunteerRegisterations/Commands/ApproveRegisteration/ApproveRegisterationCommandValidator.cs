using FluentValidation;

namespace Eghatha.Application.Features.VolunteerRegisterations.Commands.ApproveRegisteration
{
    public class ApproveRegisterationCommandValidator : AbstractValidator<ApproveRegisterationCommand>
    {
        public ApproveRegisterationCommandValidator()
        {

            RuleFor(x => x.RegisterationId).NotEmpty().NotNull();

           

        }
    }

}

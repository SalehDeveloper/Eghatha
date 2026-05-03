using FluentValidation;

namespace Eghatha.Application.Features.Teams.Commands.UpdateTeamStatus
{
    public class UpdateTeamStatusCommandValidator
    : AbstractValidator<UpdateTeamStatusCommand>
    {
        public UpdateTeamStatusCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Team Id is required");

          
        }
    }
}

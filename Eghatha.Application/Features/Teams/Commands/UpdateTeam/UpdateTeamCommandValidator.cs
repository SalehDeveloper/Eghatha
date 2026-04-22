using FluentValidation;

namespace Eghatha.Application.Features.Teams.Commands.UpdateTeam
{
    public class UpdateTeamCommandValidator : AbstractValidator<UpdateTeamCommand>
    {
        public UpdateTeamCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .When(x => x.Name is not null);

            RuleFor(x => x.Province)
                .NotEmpty()
                .When(x => x.Province is not null);

            RuleFor(x => x.City)
                .NotEmpty()
                .When(x => x.City is not null);
        }
    }
}

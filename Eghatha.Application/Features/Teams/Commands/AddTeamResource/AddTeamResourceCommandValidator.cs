using FluentValidation;

namespace Eghatha.Application.Features.Teams.Commands.AddTeamResource
{
    public class AddTeamResourceCommandValidator
    : AbstractValidator<AddTeamResourceCommand>
    {
        public AddTeamResourceCommandValidator()
        {
            RuleFor(x => x.TeamId)
                .NotEmpty();



            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .LessThanOrEqualTo(100);
               
        }
    }

}

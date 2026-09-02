using FluentValidation;

namespace Eghatha.Application.Features.Teams.Commands.DecreaseTeamReosurce
{
    public class DecreaseTeamResourceCommandValidator
    : AbstractValidator<DecreaseTeamResourceCommand>
    {
        public DecreaseTeamResourceCommandValidator()
        {
            RuleFor(x => x.TeamId)
                .NotEmpty();

            RuleFor(x => x.ResourceId)
                .NotEmpty();
            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .LessThanOrEqualTo(100);
               
        }
    }
}

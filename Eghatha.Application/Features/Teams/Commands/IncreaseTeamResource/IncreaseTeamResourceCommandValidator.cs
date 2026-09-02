using FluentValidation;

namespace Eghatha.Application.Features.Teams.Commands.IncreaseTeamResource
{
    public class IncreaseTeamResourceCommandValidator
    : AbstractValidator<IncreaseTeamResourceCommand>
    {
        public IncreaseTeamResourceCommandValidator()
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

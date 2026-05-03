using FluentValidation;

namespace Eghatha.Application.Features.Teams.Commands.IncreaseTeamResource
{
    public class IncreaseTeamResourceCommandValidator
    : AbstractValidator<IncreaseTeamResourceCommand>
    {
        public IncreaseTeamResourceCommandValidator()
        {
            RuleFor(x => x.TeamId)
                .NotEmpty().WithMessage("TeamId is required");

            RuleFor(x => x.ResourceId)
                .NotEmpty().WithMessage("ResourceId is required");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0")
                .LessThanOrEqualTo(100)
                .WithMessage("Quantity is too large");
        }
    }
}

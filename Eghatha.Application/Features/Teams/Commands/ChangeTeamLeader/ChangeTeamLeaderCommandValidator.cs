using FluentValidation;

namespace Eghatha.Application.Features.Teams.Commands.ChangeTeamLeader
{
    public class ChangeTeamLeaderCommandValidator
    : AbstractValidator<ChangeTeamLeaderCommand>
    {
        public ChangeTeamLeaderCommandValidator()
        {
            RuleFor(x => x.TeamId)
                .NotEmpty().WithMessage("TeamId is required");

            RuleFor(x => x.MemberId)
                .NotEmpty().WithMessage("MemberId is required");

         
        }
    }
}

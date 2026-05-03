using FluentValidation;

namespace Eghatha.Application.Features.Teams.Commands.DeActivateTeamMember
{
    public class UpdateTeamMemberStatusCommandValidator
   : AbstractValidator<UpdateTeamMemberStatusCommand>
    {
        public UpdateTeamMemberStatusCommandValidator()
        {
            RuleFor(x => x.TeamId)
                .NotEmpty().WithMessage("TeamId is required");

            RuleFor(x => x.MemeberId)
                .NotEmpty().WithMessage("MemberId is required");

         
        }
    }
}

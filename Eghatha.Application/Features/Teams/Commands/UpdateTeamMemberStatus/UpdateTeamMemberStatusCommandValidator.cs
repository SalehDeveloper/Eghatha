using FluentValidation;

namespace Eghatha.Application.Features.Teams.Commands.DeActivateTeamMember
{
    public class UpdateTeamMemberStatusCommandValidator
   : AbstractValidator<UpdateTeamMemberStatusCommand>
    {
        public UpdateTeamMemberStatusCommandValidator()
        {
            RuleFor(x => x.TeamId)
                .NotEmpty();

            RuleFor(x => x.MemeberId)
                .NotEmpty();

         
        }
    }
}

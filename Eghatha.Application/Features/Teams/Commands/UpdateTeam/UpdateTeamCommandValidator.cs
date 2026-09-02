using Eghatha.Application.Features.Disasters.Commands.CreateDisaster;
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

            // Coordinates
            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90);

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180);

          
        }
    

    }
}

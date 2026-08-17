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

            RuleFor(x => x)
               .Must(BeWithinSyria)
               .WithMessage("Location must be within Syria.");
        }
        private static bool BeWithinSyria(UpdateTeamCommand command)
        {
            // Approximate Syria borders
            const double minLat = 32.3;
            const double maxLat = 37.4;
            const double minLon = 35.7;
            const double maxLon = 42.4;

            return command.Latitude >= minLat &&
                   command.Latitude <= maxLat &&
                   command.Longitude >= minLon &&
                   command.Longitude <= maxLon;
        }

    }
}

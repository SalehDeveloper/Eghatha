using Eghatha.Domain.Teams;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Commands.CreateTeam
{
    public class CreateTeamCommandValidator : AbstractValidator<CreateTeamCommand>
    {
        public CreateTeamCommandValidator()
        {
            RuleFor(x => x.Name)
             .NotEmpty();

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90);

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180);

            // Syria bounding box validation
            RuleFor(x => x)
                .Must(BeWithinSyria)
                .WithMessage("Location must be within Syria.");
        }

        private static bool BeWithinSyria(CreateTeamCommand command)
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

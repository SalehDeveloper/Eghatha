using Eghatha.Application.Features.Volunteers.Commands.CreateVolunteer;
using FluentValidation;

namespace Eghatha.Application.Features.Volunteers.Commands.UpdateLocation
{
    public sealed class UpdateVolunteerLocationCommandValidator
    : AbstractValidator<UpdateVolunteerLocationCommand>
    {
        public UpdateVolunteerLocationCommandValidator()
        {
            RuleFor(x => x.VolunteerId)
                .NotEmpty()
                .WithMessage("VolunteerId is required.");

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90)
                .WithMessage("Latitude must be between -90 and 90.");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180)
                .WithMessage("Longitude must be between -180 and 180.");


            RuleFor(x => x)
              .Must(BeWithinSyria)
              .WithMessage("Location must be within Syria.");
        }

        private static bool BeWithinSyria(UpdateVolunteerLocationCommand command)
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

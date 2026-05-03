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
        }
    }
}

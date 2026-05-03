using FluentValidation;

namespace Eghatha.Application.Features.Volunteers.Commands.UpdateStatus
{
    public sealed class UpdateVolunteerStatusCommandValidator
    : AbstractValidator<UpdateVolunteerStatusCommand>
    {
        public UpdateVolunteerStatusCommandValidator()
        {
            RuleFor(x => x.VolunteerId)
                .NotEmpty()
                .WithMessage("VolunteerId is required.");

          
        }
    }
}

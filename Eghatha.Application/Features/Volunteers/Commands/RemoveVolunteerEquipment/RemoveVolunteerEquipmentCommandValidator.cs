using FluentValidation;

namespace Eghatha.Application.Features.Volunteers.Commands.RemoveVolunteerEquipment
{
    public sealed class RemoveVolunteerEquipmentCommandValidator
    : AbstractValidator<RemoveVolunteerEquipmentCommand>
    {
        public RemoveVolunteerEquipmentCommandValidator()
        {
            RuleFor(x => x.VolunteerId)
                .NotEmpty()
                .WithMessage("VolunteerId is required.");

            RuleFor(x => x.EquipmentId)
                .NotEmpty()
                .WithMessage("EquipmentId is required.");
        }
    }
}

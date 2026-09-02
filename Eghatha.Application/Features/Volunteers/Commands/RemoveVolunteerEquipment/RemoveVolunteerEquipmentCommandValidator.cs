using FluentValidation;

namespace Eghatha.Application.Features.Volunteers.Commands.RemoveVolunteerEquipment
{
    public sealed class RemoveVolunteerEquipmentCommandValidator
    : AbstractValidator<RemoveVolunteerEquipmentCommand>
    {
        public RemoveVolunteerEquipmentCommandValidator()
        {
            RuleFor(x => x.VolunteerId)
                .NotEmpty();

            RuleFor(x => x.EquipmentId)
                .NotEmpty();
        }
    }
}

using FluentValidation;

namespace Eghatha.Application.Features.Volunteers.Commands.UpdateEquipmentStatus
{
    public sealed class UpdateVolunteerEquipmentStatusCommandValidator
        : AbstractValidator<UpdateVolunteerEquipmentStatusCommand>
    {
        public UpdateVolunteerEquipmentStatusCommandValidator()
        {
            RuleFor(x => x.VolunteerId)
                .NotEmpty();

            RuleFor(x => x.EquipmentId)
                .NotEmpty();

        }
    }
}

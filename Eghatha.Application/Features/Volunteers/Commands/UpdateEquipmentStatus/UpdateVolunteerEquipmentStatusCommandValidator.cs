using FluentValidation;

namespace Eghatha.Application.Features.Volunteers.Commands.UpdateEquipmentStatus
{
    public sealed class UpdateVolunteerEquipmentStatusCommandValidator
        : AbstractValidator<UpdateVolunteerEquipmentStatusCommand>
    {
        public UpdateVolunteerEquipmentStatusCommandValidator()
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

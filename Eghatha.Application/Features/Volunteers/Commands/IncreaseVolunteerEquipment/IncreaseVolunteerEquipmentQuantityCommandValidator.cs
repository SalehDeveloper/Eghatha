using FluentValidation;

namespace Eghatha.Application.Features.Volunteers.Commands.IncreaseVolunteerEquipment
{
    public sealed class IncreaseVolunteerEquipmentQuantityCommandValidator
    : AbstractValidator<IncreaseVolunteerEquipmentQuantityCommand>
    {
        public IncreaseVolunteerEquipmentQuantityCommandValidator()
        {
            RuleFor(x => x.VolunteerId)
                .NotEmpty()
                .WithMessage("VolunteerId is required.");

            RuleFor(x => x.EquipmentId)
                .NotEmpty()
                .WithMessage("EquipmentId is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.");
        }
    }
}

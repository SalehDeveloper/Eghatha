using FluentValidation;

namespace Eghatha.Application.Features.Volunteers.Commands.DecreaseEquipmentQuantity
{
    public sealed class DecreaseVolunteerEquipmentQuantityCommandValidator
    : AbstractValidator<DecreaseVolunteerEquipmentQuantityCommand>
    {
        public DecreaseVolunteerEquipmentQuantityCommandValidator()
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

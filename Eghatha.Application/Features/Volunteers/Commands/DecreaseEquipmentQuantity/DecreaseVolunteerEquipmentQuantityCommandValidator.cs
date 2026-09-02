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
               ;

            RuleFor(x => x.EquipmentId)
                .NotEmpty()
               ;

            RuleFor(x => x.Quantity)
                .GreaterThan(0);
        }
    }
}

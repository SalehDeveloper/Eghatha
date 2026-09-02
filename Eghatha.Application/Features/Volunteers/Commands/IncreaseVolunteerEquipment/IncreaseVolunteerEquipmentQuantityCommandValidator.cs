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
                ;

            RuleFor(x => x.EquipmentId)
                .NotEmpty()
                ;

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                ;
        }
    }
}

using FluentValidation;

namespace Eghatha.Application.Features.Volunteers.Commands.AddVolunteerEquipment
{
    public sealed class AddVolunteerEquipmentCommandValidator
    : AbstractValidator<AddVolunteerEquipmentCommand>
    {
        public AddVolunteerEquipmentCommandValidator()
        {
            RuleFor(x => x.VolunteerId)
                .NotEmpty()
               ;

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

          

            RuleFor(x => x.Quantity)
                .GreaterThan(0);
        }
    }
}

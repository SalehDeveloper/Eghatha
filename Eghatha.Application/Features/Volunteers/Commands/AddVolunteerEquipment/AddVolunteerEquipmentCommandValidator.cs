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
                .WithMessage("VolunteerId is required.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Equipment name is required.")
                .MaximumLength(100)
                .WithMessage("Equipment name must not exceed 100 characters.");

          

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.");
        }
    }
}

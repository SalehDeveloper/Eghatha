using FluentValidation;

namespace Eghatha.Application.Features.Volunteers.Commands.UpdateVolunteerEquipment
{
    public sealed class UpdateVolunteerEquipmentCommandValidator
    : AbstractValidator<UpdateVolunteerEquipmentCommand>
    {
        public UpdateVolunteerEquipmentCommandValidator()
        {
            RuleFor(x => x.VolunteerId)
                .NotEmpty()
                .WithMessage("VolunteerId is required.");

            RuleFor(x => x.EquipmentId)
                .NotEmpty()
                .WithMessage("EquipmentId is required.");

            When(x => x.Name is not null, () =>
            {
                RuleFor(x => x.Name)
                    .NotEmpty()
                    .WithMessage("Name cannot be empty when provided.")
                    .MaximumLength(100)
                    .WithMessage("Name must not exceed 100 characters.");
            });

            When(x => x.Quantity is not null, () =>
            {
                RuleFor(x => x.Quantity)
                    .GreaterThan(0)
                    .WithMessage("Quantity must be greater than 0.");
            });
        }
    }
}

using FluentValidation;

namespace Eghatha.Application.Features.Volunteers.Commands.UpdateVolunteerEquipment
{
    public sealed class UpdateVolunteerEquipmentCommandValidator
    : AbstractValidator<UpdateVolunteerEquipmentCommand>
    {
        public UpdateVolunteerEquipmentCommandValidator()
        {
            RuleFor(x => x.VolunteerId)
                .NotEmpty();

            RuleFor(x => x.EquipmentId)
                .NotEmpty();

            When(x => x.Name is not null, () =>
            {
                RuleFor(x => x.Name)
                    .NotEmpty()
                    
                    .MaximumLength(100)
                    ;
            });

            When(x => x.Quantity is not null, () =>
            {
                RuleFor(x => x.Quantity)
                    .GreaterThan(0)
                    ;
            });
        }
    }
}

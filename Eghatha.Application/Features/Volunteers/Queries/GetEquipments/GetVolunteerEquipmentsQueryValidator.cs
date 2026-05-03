using FluentValidation;

namespace Eghatha.Application.Features.Volunteers.Queries.GetEquipments
{
    public sealed class GetVolunteerEquipmentsQueryValidator
    : AbstractValidator<GetVolunteerEquipmentsQuery>
    {
        public GetVolunteerEquipmentsQueryValidator()
        {
            RuleFor(x => x.VolunteerId)
                .NotEmpty()
                .WithMessage("VolunteerId is required.");

        
        }
    }
}

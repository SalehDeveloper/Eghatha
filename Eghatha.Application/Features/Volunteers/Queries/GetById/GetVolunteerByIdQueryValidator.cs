using FluentValidation;

namespace Eghatha.Application.Features.Volunteers.Queries.GetById
{
    public sealed class GetVolunteerByIdQueryValidator
    : AbstractValidator<GetVolunteerByIdQuery>
    {
        public GetVolunteerByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Volunteer Id is required.");
        }
    }
}

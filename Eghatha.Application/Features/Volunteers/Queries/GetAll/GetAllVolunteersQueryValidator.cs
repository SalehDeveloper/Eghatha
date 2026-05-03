using FluentValidation;

namespace Eghatha.Application.Features.Volunteers.Queries.GetAll
{
    public sealed class GetAllVolunteersQueryValidator
    : AbstractValidator<GetAllVolunteersQuery>
    {
        public GetAllVolunteersQueryValidator()
        {
            
            When(x => !string.IsNullOrWhiteSpace(x.SearchTerm), () =>
            {
                RuleFor(x => x.SearchTerm)
                    .MaximumLength(100)
                    .WithMessage("SearchTerm must not exceed 100 characters.");
            });

          

            When(x => !string.IsNullOrWhiteSpace(x.Province), () =>
            {
                RuleFor(x => x.Province)
                    .MaximumLength(100)
                    .WithMessage("Province must not exceed 100 characters.");
            });

            When(x => !string.IsNullOrWhiteSpace(x.City), () =>
            {
                RuleFor(x => x.City)
                    .MaximumLength(100)
                    .WithMessage("City must not exceed 100 characters.");
            });
        }
    }
}

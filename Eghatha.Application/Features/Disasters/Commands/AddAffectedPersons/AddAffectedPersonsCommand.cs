using Eghatha.Application.Features.Disasters.Dtos;
using Eghatha.Domain.Disasters;
using ErrorOr;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Commands.AddAffectedPersons
{
    public sealed record AddAffectedPersonsCommand(Guid DisasterId, List<AffectedPersonDto> Persons)
        : IRequest<ErrorOr<Success>>;

    public class AddAffectedPersonCommandValidator : AbstractValidator<AddAffectedPersonsCommand>
    {
        public AddAffectedPersonCommandValidator()
        {
            RuleFor(x => x.Persons)
            .NotEmpty()
            .WithMessage("At least one affected person is required.");

            RuleForEach(x => x.Persons).ChildRules(person =>
            {
                person.RuleFor(p => p.Name)
                    .NotEmpty();

                person.RuleFor(p => p.Age)
                    .InclusiveBetween(0, 120);

                person.RuleFor(p => p.Phone)
                    .NotEmpty()
                    .Matches(@"^09\d{8}$")
                    .WithMessage("Phone number must be a valid Syrian mobile number (e.g. 0991234567).");

                person.RuleFor(p => p.Status)
                    .NotEmpty();

            });
    }
    }
}
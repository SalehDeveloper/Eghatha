using Eghatha.Application.Features.Disasters.Dtos;
using Eghatha.Application.Features.Teams.Commands.CreateTeam;
using Eghatha.Domain.Disasters;
using ErrorOr;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Commands.CreateDisaster
{
    public sealed record CreateDisasterCommand(
     string Title,
     string Description,
     double Latitude,
     double Longitude,
     DisasterType DisasterType,
     string? CustomTypeDescription,
     string ReporterName,
     string ReporterPhone,
     string ReporterNationalId)

     : IRequest<ErrorOr<CreateDisasterDto>>;

    public sealed class CreateDisasterCommandValidator
    : AbstractValidator<CreateDisasterCommand>
    {
        public CreateDisasterCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(2000);

            // Coordinates
            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90);

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180);

            RuleFor(x => x)
               .Must(BeWithinSyria)
               .WithMessage("Location must be within Syria.");
            // Reporter
            RuleFor(x => x.ReporterName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.ReporterPhone)
                .NotEmpty()
                .Matches(@"^09\d{8}$")
                .WithMessage("Phone number must be a valid Syrian mobile number.");

            RuleFor(x => x.ReporterNationalId)
                .NotEmpty()
                .Matches(@"^\d{11}$")
                .WithMessage("National ID must contain exactly 11 digits.");

          
        }

        private static bool BeWithinSyria(CreateDisasterCommand command)
        {
            // Approximate Syria borders
            const double minLat = 32.3;
            const double maxLat = 37.4;
            const double minLon = 35.7;
            const double maxLon = 42.4;

            return command.Latitude >= minLat &&
                   command.Latitude <= maxLat &&
                   command.Longitude >= minLon &&
                   command.Longitude <= maxLon;
        }
    }

}

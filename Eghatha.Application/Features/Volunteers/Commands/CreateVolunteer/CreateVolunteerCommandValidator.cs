using Eghatha.Application.Features.Teams.Commands.CreateTeam;
using FluentValidation;

namespace Eghatha.Application.Features.Volunteers.Commands.CreateVolunteer
{
    public class CreateVolunteerCommandValidator
    : AbstractValidator<CreateVolunteerCommand>
    {
        public CreateVolunteerCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(100);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress();
            RuleFor(x => x.PhoneNumber)
                          .NotEmpty()
                          .WithMessage("Phone number is required.")
                          .Matches(@"^09\d{8}$")
                          .WithMessage("Phone number must be a valid Syrian mobile number (e.g. 0991234567).");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8);

       

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90)
                .WithMessage("Latitude must be between -90 and 90");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180)
                .WithMessage("Longitude must be between -180 and 180");


            RuleFor(x => x)
              .Must(BeWithinSyria)
              .WithMessage("Location must be within Syria.");

            RuleFor(x => x.YearsOfExperience)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(60);

        
            RuleFor(x => x.photo)
                .NotNull().WithMessage("Photo is required")
                .Must(file => file.Length > 0)
                .WithMessage("Photo cannot be empty")
                .Must(file => file.ContentType.StartsWith("image/"))
                .WithMessage("Photo must be an image")
                .Must(file => file.Length <= 5 * 1024 * 1024) 
                .WithMessage("Photo size must not exceed 5MB");

          
            RuleFor(x => x.Cv)
                .NotNull().WithMessage("CV is required")
                .Must(file => file.Length > 0)
                .WithMessage("CV cannot be empty")
                .Must(file => file.ContentType == "application/pdf")
                .WithMessage("CV must be a PDF file")
                .Must(file => file.Length <= 10 * 1024 * 1024) 
                .WithMessage("CV size must not exceed 10MB");
        }

        private static bool BeWithinSyria(CreateVolunteerCommand command)
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




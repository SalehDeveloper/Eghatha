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
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.PhoneNumber)
                          .NotEmpty()
                          
                          .Matches(@"^09\d{8}$")
                         ;

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8);

       

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90)
                ;

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180)
                ;


         
            RuleFor(x => x.YearsOfExperience)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(60);

        
            RuleFor(x => x.photo)
               .NotNull()
                .Must(file => file.Length > 0)
               
                .Must(file => file.ContentType.StartsWith("image/"))
              
                .Must(file => file.Length <= 5 * 1024 * 1024) 
               ;

          
            RuleFor(x => x.Cv)
                .NotNull()
                .Must(file => file.Length > 0)
               
                .Must(file => file.ContentType == "application/pdf")
               
                .Must(file => file.Length <= 10 * 1024 * 1024) 
                ;
        }

     
    }

}




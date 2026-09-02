using Eghatha.Application.Features.Volunteers.Commands.CreateVolunteer;
using FluentValidation;

namespace Eghatha.Application.Features.Volunteers.Commands.UpdateLocation
{
    public sealed class UpdateVolunteerLocationCommandValidator
    : AbstractValidator<UpdateVolunteerLocationCommand>
    {
        public UpdateVolunteerLocationCommandValidator()
        {
            RuleFor(x => x.VolunteerId)
                .NotEmpty()
               ;

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90)
                ;

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180)
                ;


         
        }

     
    }
}

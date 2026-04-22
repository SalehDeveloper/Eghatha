using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Commands.UpdateLiveTeamLocation
{
    public class UpdateLiveTeamLocationCommandValidator:AbstractValidator<UpdateLiveTeamLocationCommand>
    {
        public UpdateLiveTeamLocationCommandValidator()
        {
            RuleFor(x => x.TeamId).NotEmpty()
              .NotNull()
              .WithMessage("Id is required");
           
            RuleFor(x => x.Latitude)
          .InclusiveBetween(-90, 90);

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180);
        }
    }
}

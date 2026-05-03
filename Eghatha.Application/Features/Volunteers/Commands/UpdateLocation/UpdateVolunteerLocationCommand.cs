using Eghatha.Domain.Volunteers;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Volunteers.Commands.UpdateLocation
{
    public record UpdateVolunteerLocationCommand(
       Guid VolunteerId,
       double Latitude,
       double Longitude
   ) : IRequest<ErrorOr<Updated>>;
}

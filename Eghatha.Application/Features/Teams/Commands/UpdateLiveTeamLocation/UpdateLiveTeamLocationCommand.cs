using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Commands.UpdateLiveTeamLocation
{
    public record UpdateLiveTeamLocationCommand(
        Guid TeamId,
        double Latitude,
        double Longitude) : IRequest<ErrorOr<Updated>>;
    
    
}

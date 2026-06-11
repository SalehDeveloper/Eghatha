using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Queries.GetCurrentTeamLocation
{
    public sealed record GetTeamCurrentLocationQuery(Guid TeamId) : IRequest<ErrorOr<TeamLocation>>;
    public sealed record TeamLocation(double Latitude, double Longitude , bool IsLive );
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Queries.GetTeamSpecialities
{
    public sealed record GetTeamSpecialitiesQuery : IRequest<IReadOnlyList<TeamSpecialityResponse>>;

    public sealed record TeamSpecialityResponse(int Value , string Name);


}

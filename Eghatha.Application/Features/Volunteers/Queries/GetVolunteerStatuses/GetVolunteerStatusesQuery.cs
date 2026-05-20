using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Volunteers.Queries.GetVolunteerStatuses
{
    public sealed record GetVolunteerStatusesQuery
     : IRequest<IReadOnlyList<VolunteerStatusResponse>>;

    public sealed record VolunteerStatusResponse(int Value, string Name);
}

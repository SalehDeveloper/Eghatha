using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Volunteers.Queries.GetVolunteerSpecialities
{
    public sealed record GetVolunteerSpecialitiesQuery
      : IRequest<IReadOnlyList<VolunteerSpecialityResponse>>;

    public sealed record VolunteerSpecialityResponse(int Value, string Name);
}

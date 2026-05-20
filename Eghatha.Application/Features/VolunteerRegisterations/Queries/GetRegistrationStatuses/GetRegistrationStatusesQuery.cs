using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.VolunteerRegisterations.Queries.GetRegistrationStatuses
{
    public sealed record GetRegistrationStatusesQuery
     : IRequest<IReadOnlyList<RegistrationStatusResponse>>;

    public sealed record RegistrationStatusResponse(int Value, string Name);
}

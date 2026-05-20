using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Queries.GetDisasterTypes
{
    public sealed record GetDisasterTypesQuery : IRequest<IReadOnlyList<DisasterTypeResponse>>;

    public sealed record DisasterTypeResponse(int Value, string Name);

}

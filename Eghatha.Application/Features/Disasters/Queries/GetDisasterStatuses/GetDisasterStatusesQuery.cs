using Eghatha.Application.Features.Disasters.Queries.GetDisasterTypes;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Queries.GetDisasterStatuses
{
    public sealed  record GetDisasterStatusesQuery : IRequest<IReadOnlyList<DisasterStatusResponse>>;
}

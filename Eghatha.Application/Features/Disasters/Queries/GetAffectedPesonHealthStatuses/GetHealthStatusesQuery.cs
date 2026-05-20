using Eghatha.Application.Features.Disasters.Queries.GetDisasterStatuses;
using Eghatha.Application.Features.Disasters.Queries.GetDisasterTypes;
using Eghatha.Domain.Disasters;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Queries.GetAffectedPesonHealthStatuses
{
    public sealed record  GetHealthStatusesQuery : IRequest<IReadOnlyList<AffectedPersonHealthStatusResponse>>;

    public sealed record AffectedPersonHealthStatusResponse(int Value, string Name);
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Queries.GetResourceStatuses
{
    public sealed record GetResourceStatusesQuery
     : IRequest<IReadOnlyList<ResourceStatusResponse>>;

    public sealed record ResourceStatusResponse(int Value, string Name);
}

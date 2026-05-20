using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Queries.GetResourceTypes
{
    public sealed record GetResourceTypesQuery
     : IRequest<IReadOnlyList<ResourceTypeResponse>>;

    public sealed record ResourceTypeResponse(
    int Value,
    string Name,
    bool IsConsumable);
}

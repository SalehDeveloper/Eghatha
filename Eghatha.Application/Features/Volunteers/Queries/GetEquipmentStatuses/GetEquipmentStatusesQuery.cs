using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Volunteers.Queries.GetEquipmentStatuses
{
    public sealed record GetEquipmentStatusesQuery
      : IRequest<IReadOnlyList<EquipmentStatusResponse>>;

    public sealed record EquipmentStatusResponse(int Value, string Name);
}

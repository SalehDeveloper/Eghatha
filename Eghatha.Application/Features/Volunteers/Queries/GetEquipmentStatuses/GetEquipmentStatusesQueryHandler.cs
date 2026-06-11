using Eghatha.Domain.Volunteers.Equipments;
using MediatR;

namespace Eghatha.Application.Features.Volunteers.Queries.GetEquipmentStatuses
{
    public sealed class GetEquipmentStatusesQueryHandler
    : IRequestHandler<GetEquipmentStatusesQuery, IReadOnlyList<EquipmentStatusResponse>>
    {
        public Task<IReadOnlyList<EquipmentStatusResponse>> Handle(
            GetEquipmentStatusesQuery request,
            CancellationToken cancellationToken)
        {
            var result = EquipmentStatus.List
                .Select(x => new EquipmentStatusResponse(x.Value, x.Name))
                 .OrderBy(x => x.Value)
                .ToList();

            return Task.FromResult<IReadOnlyList<EquipmentStatusResponse>>(result);
        }
    }
}

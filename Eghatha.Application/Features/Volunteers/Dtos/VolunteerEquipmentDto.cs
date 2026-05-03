using Eghatha.Domain.Volunteers.Equipments;

namespace Eghatha.Application.Features.Volunteers.Dtos
{
    public sealed record VolunteerEquipmentDto(
    Guid Id,
    string Name,
    EquipmentCategory Category,
    int Quantity,
    EquipmentStatus Status
);

}

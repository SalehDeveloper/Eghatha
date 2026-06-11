using Eghatha.Domain.Volunteers.Equipments;

namespace Eghatha.Application.Features.Volunteers.Dtos
{
    public sealed record VolunteerEquipmentDto(
    Guid Id,
    string Name,
    string Category,
    int Quantity,
    string Status
);

}

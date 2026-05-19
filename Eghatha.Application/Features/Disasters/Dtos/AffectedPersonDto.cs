using Eghatha.Domain.Disasters.AffectedPersons;

namespace Eghatha.Application.Features.Disasters.Dtos
{
    public sealed record AffectedPersonDto(
    string Name,
    int Age,
    string Phone,
    HealthStatus Status,
    string? Notes);


}

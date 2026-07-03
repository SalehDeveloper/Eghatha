using Eghatha.Domain.Disasters.AffectedPersons;

namespace Eghatha.Application.Features.Disasters.Dtos
{
    public sealed record AffectedPersonDto(
    string Name,
    int Age,
    string Phone,
    string Status,
    string? Notes);
}

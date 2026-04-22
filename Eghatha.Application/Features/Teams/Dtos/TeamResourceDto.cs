namespace Eghatha.Application.Features.Teams.Dtos
{
    public record TeamResourceDto(
    Guid Id,
    string Type,
    int Quantity,
    string Status,
    bool IsConsumable
);


}

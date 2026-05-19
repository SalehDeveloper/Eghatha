namespace Eghatha.Application.Features.Disasters.Dtos
{
    public sealed record ResourceDto(
    Guid Id,
    string ResourceType,
    int Sent,
    int Consumed,
    int Returned,
    int Damaged,
    string? Notes
);
}

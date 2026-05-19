namespace Eghatha.Application.Features.Disasters.Dtos
{
    public sealed record DisasterTimelineDto(
    Guid Id,
    string EventType,
    string Description,
    DateTimeOffset OccurredAt
);
}

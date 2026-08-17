namespace Eghatha.Application.Features.Disasters.Dtos
{
    public record SpamCheckResultDto(
        bool IsSpam,
        Guid? MatchedDisasterId,
        double Confidence,
        string Reasoning);
}

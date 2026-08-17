namespace Eghatha.Contract.Disasters.Responses
{
    public record SpamCheckResponse(
      bool IsSpam,
      Guid? MatchedDisasterId,
      double Confidence,
      string Reasoning);

}

namespace Eghatha.Contract.Identity.Responses
{
    public sealed record TokenResponse(string? AccessToken, string? RefreshToken, DateTime ExpiresOnUtc);
}

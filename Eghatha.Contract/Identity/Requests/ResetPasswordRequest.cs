namespace Eghatha.Contract.Identity.Requests
{
    public record ResetPasswordRequest(string Email, string Code, string NewPassword);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Services
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string toEmail, string otpCode, int expirationMinutes);
        Task SendConfirmEmailAsync(string toEmail, string otpCode, int expirationMinutes);
        Task SendTeamInvitationEmailAsync(string toEmail, string fullName, string teamName, string otpCode, int expirationMinutes);
        Task SendVolunteerApprovedEmailAsync(string toEmail, string fullName);

        Task SendVolunteerRejectedEmailAsync(
    string toEmail,
    string fullName,
    string? reason);
    }
        
}

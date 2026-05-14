using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Services
{
    public interface IEmailTemplateBuilder
    {
        string BuildPasswordResetEmail(string otpCode, int expirationMinutes);
        string BuildEmailConfirmationTemplate(string otpCode, int expirationMinutes);

        string BuildTeamInvitationEmail(string fullName,string teamName,string otpCode,int expirationMinutes);

        string BuildVolunteerApprovedEmail(string fullName);

        string BuildVolunteerRejectedEmail(string fullName, string? reason);

        string BuildDisasterAssignmentEmail(string volunteerName, double latitude, double longitude, string title, string type, string city, string province, DateTimeOffset startTime, string description);
    }
}

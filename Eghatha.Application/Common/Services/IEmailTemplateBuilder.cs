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
    }
}

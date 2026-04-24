using Eghatha.Application.Common.Services;
using Microsoft.Extensions.Options;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace Eghatha.Infastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions _emailOptions;
        private readonly IEmailTemplateBuilder _templateBuilder;

        public EmailService(IOptions<EmailOptions> emailOptions, IEmailTemplateBuilder templateBuilder)
        {
            _emailOptions = emailOptions.Value;
            _templateBuilder = templateBuilder;
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string otpCode, int expirationMinutes)
        {
            var subject = "🔐 Reset Your Eghatha Password";
            var body = _templateBuilder.BuildPasswordResetEmail(otpCode, expirationMinutes);

            await SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendConfirmEmailAsync(string toEmail, string otpCode, int expirationMinutes)
        {
            var subject = "🔐 Confirm Your Email";
            var body = _templateBuilder.BuildEmailConfirmationTemplate(otpCode, expirationMinutes);

            await SendEmailAsync(toEmail, subject, body);
        }

        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var emailMessage = new MimeMessage();

            // Sender with better display name
            emailMessage.From.Add(new MailboxAddress("Eghatha Support", _emailOptions.From));
            emailMessage.To.Add(MailboxAddress.Parse(toEmail));
            emailMessage.Subject = subject;

            // Create HTML body
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body,
                TextBody = StripHtmlTags(body) // Plain text fallback
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                    await client.ConnectAsync(_emailOptions.SmtpServer, _emailOptions.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailOptions.UserName, _emailOptions.Password);
                    await client.SendAsync(emailMessage);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to send email: {ex.Message}");
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }

        public async Task SendTeamInvitationEmailAsync(string toEmail,string fullName,string teamName,string otpCode,int expirationMinutes)
        {
            var subject = "🎉 You've been invited to join a team";

            var body = _templateBuilder.BuildTeamInvitationEmail(
                fullName,
                teamName,
                otpCode,
                expirationMinutes);

            await SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendVolunteerApprovedEmailAsync(string toEmail, string fullName)
        {
            var subject = "🎉 Your Eghatha Volunteer Account Has Been Approved";
            var body = _templateBuilder.BuildVolunteerApprovedEmail(fullName);

            await SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendVolunteerRejectedEmailAsync(
    string toEmail,
    string fullName,
    string? reason)
        {
            var subject = "❌ Your Eghatha Volunteer Application Status";
            var body = _templateBuilder.BuildVolunteerRejectedEmail(fullName, reason);

            await SendEmailAsync(toEmail, subject, body);
        }
        private string StripHtmlTags(string html)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;

            var plainText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]*>", "");
            plainText = System.Text.RegularExpressions.Regex.Replace(plainText, "\\s+", " ");
            return plainText.Trim();
        }
    }

        public class EmailOptions
        {
            public string From { get; set; }

            public string SmtpServer { get; set; }

            public int Port { get; set; }

            public string UserName { get; set; }

            public string Password { get; set; }
        }
}


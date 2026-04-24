using Eghatha.Application.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Services
{
    public class EmailTemplateBuilder : IEmailTemplateBuilder
    {
        public string BuildPasswordResetEmail(string otpCode, int expirationMinutes)
        {
        
            return $@"
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Password Reset</title>
        </head>
        <body style='margin: 0; padding: 0; font-family: 'Segoe UI', Arial, sans-serif; background-color: #f4f4f5;'>
            <table align='center' border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 16px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.1);'>
                <!-- Header -->
                <tr>
                    <td style='background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 40px 30px; text-align: center;'>
                        <h1 style='color: #ffffff; margin: 0; font-size: 28px; font-weight: 600; letter-spacing: -0.5px;'>
                            🔐 Eghatha
                        </h1>
                        <p style='color: rgba(255,255,255,0.9); margin: 10px 0 0; font-size: 16px;'>
                            Password Reset Request
                        </p>
                    </td>
                </tr>
                
                <!-- Content -->
                <tr>
                    <td style='padding: 40px 30px; background-color: #ffffff;'>
                        <h2 style='color: #1f2937; margin: 0 0 20px; font-size: 24px; font-weight: 600;'>
                            Reset Your Password
                        </h2>
                        
                        <p style='color: #4b5563; line-height: 1.6; margin: 0 0 20px; font-size: 16px;'>
                            We received a request to reset the password for your Eghatha account. 
                            Use the verification code below to proceed with resetting your password.
                        </p>
                        
                        <!-- OTP Code Box -->
                        <div style='background-color: #f3f4f6; border-radius: 12px; padding: 25px; text-align: center; margin: 30px 0; border: 2px dashed #d1d5db;'>
                            <p style='color: #6b7280; margin: 0 0 10px; font-size: 14px; letter-spacing: 1px; text-transform: uppercase; font-weight: 500;'>
                                Your Verification Code
                            </p>
                            <div style='font-size: 48px; font-weight: 700; color: #4f46e5; letter-spacing: 8px; font-family: monospace; background-color: #ffffff; display: inline-block; padding: 15px 30px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.05);'>
                                {otpCode}
                            </div>
                        </div>
                        
                        <!-- Validity Info -->
                        <div style='background-color: #fef3c7; border-left: 4px solid #f59e0b; padding: 15px 20px; border-radius: 8px; margin: 20px 0;'>
                            <p style='margin: 0; color: #92400e; font-size: 14px; display: flex; align-items: center; gap: 10px;'>
                                <span style='font-size: 20px;'>⏰</span>
                                <span>This code will expire in <strong>{expirationMinutes} minutes</strong> for security reasons.</span>
                            </p>
                        </div>
                        
                        <!-- Security Note -->
                        <div style='background-color: #f0fdf4; border-left: 4px solid #22c55e; padding: 15px 20px; border-radius: 8px; margin: 20px 0;'>
                            <p style='margin: 0; color: #166534; font-size: 14px; display: flex; align-items: center; gap: 10px;'>
                                <span style='font-size: 20px;'>🔒</span>
                                <span>If you didn't request this password reset, you can safely ignore this email.</span>
                            </p>
                        </div>
                        
                        <!-- Instructions -->
                        <div style='margin: 30px 0 20px;'>
                            <p style='color: #4b5563; font-size: 14px; line-height: 1.6;'>
                                <strong>How to reset your password:</strong>
                            </p>
                            <ol style='color: #4b5563; font-size: 14px; line-height: 1.8; margin-top: 10px; padding-left: 20px;'>
                                <li>Copy the verification code above</li>
                                <li>Return to Eghatha and enter the code</li>
                                <li>Create your new password</li>
                                <li>Log in with your new credentials</li>
                            </ol>
                        </div>
                    </td>
                </tr>
                
                <!-- Footer -->
                <tr>
                    <td style='background-color: #f9fafb; padding: 30px; text-align: center; border-top: 1px solid #e5e7eb;'>
                        <p style='margin: 0 0 10px; color: #6b7280; font-size: 12px;'>
                            This is an automated message, please do not reply to this email.
                        </p>
                        <p style='margin: 0; color: #9ca3af; font-size: 12px;'>
                            &copy; 2024 Eghatha. All rights reserved.
                        </p>
                        <div style='margin-top: 15px;'>
                            <a href='#' style='color: #6b7280; text-decoration: none; margin: 0 10px; font-size: 12px;'>Privacy Policy</a>
                            <a href='#' style='color: #6b7280; text-decoration: none; margin: 0 10px; font-size: 12px;'>Terms of Service</a>
                            <a href='#' style='color: #6b7280; text-decoration: none; margin: 0 10px; font-size: 12px;'>Contact Support</a>
                        </div>
                    </td>
                </tr>
            </table>
        </body>
        </html>";
        }

        public string BuildEmailConfirmationTemplate(string otpCode, int expirationMinutes)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Email Confirmation</title>
</head>
<body style='margin: 0; padding: 0; font-family: 'Segoe UI', Arial, sans-serif; background-color: #f4f4f5;'>
    <table align='center' border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 16px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.1);'>

        <!-- Header -->
        <tr>
            <td style='background: linear-gradient(135deg, #4f46e5 0%, #06b6d4 100%); padding: 40px 30px; text-align: center;'>
                <h1 style='color: #ffffff; margin: 0; font-size: 28px; font-weight: 600; letter-spacing: -0.5px;'>
                    🔐 Eghatha
                </h1>
                <p style='color: rgba(255,255,255,0.9); margin: 10px 0 0; font-size: 16px;'>
                    Email Confirmation
                </p>
            </td>
        </tr>

        <!-- Content -->
        <tr>
            <td style='padding: 40px 30px; background-color: #ffffff;'>
                
                <h2 style='color: #1f2937; margin: 0 0 20px; font-size: 24px; font-weight: 600;'>
                    Confirm Your Email Address
                </h2>

                <p style='color: #4b5563; line-height: 1.6; margin: 0 0 20px; font-size: 16px;'>
                    Welcome! Please confirm your email address to activate your account and start using Eghatha.
                </p>

                <!-- OTP Code Box -->
                <div style='background-color: #f3f4f6; border-radius: 12px; padding: 25px; text-align: center; margin: 30px 0; border: 2px dashed #d1d5db;'>
                    <p style='color: #6b7280; margin: 0 0 10px; font-size: 14px; letter-spacing: 1px; text-transform: uppercase; font-weight: 500;'>
                        Your Verification Code
                    </p>

                    <div style='font-size: 48px; font-weight: 700; color: #4f46e5; letter-spacing: 8px; font-family: monospace; background-color: #ffffff; display: inline-block; padding: 15px 30px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.05);'>
                        {otpCode}
                    </div>
                </div>

                <!-- Expiration -->
                <div style='background-color: #fef3c7; border-left: 4px solid #f59e0b; padding: 15px 20px; border-radius: 8px; margin: 20px 0;'>
                    <p style='margin: 0; color: #92400e; font-size: 14px;'>
                        ⏰ This code will expire in <strong>{expirationMinutes} minutes</strong>.
                    </p>
                </div>

                <!-- Security Note -->
                <div style='background-color: #f0fdf4; border-left: 4px solid #22c55e; padding: 15px 20px; border-radius: 8px; margin: 20px 0;'>
                    <p style='margin: 0; color: #166534; font-size: 14px;'>
                        🔒 If you didn’t create an account, you can safely ignore this email.
                    </p>
                </div>

                <!-- Instructions -->
                <div style='margin: 30px 0 20px;'>
                    <p style='color: #4b5563; font-size: 14px; line-height: 1.6;'>
                        <strong>How to confirm your email:</strong>
                    </p>
                    <ol style='color: #4b5563; font-size: 14px; line-height: 1.8; margin-top: 10px; padding-left: 20px;'>
                        <li>Copy the verification code above</li>
                        <li>Return to Eghatha confirmation page</li>
                        <li>Enter the code</li>
                        <li>Complete your account activation</li>
                    </ol>
                </div>

            </td>
        </tr>

        <!-- Footer -->
        <tr>
            <td style='background-color: #f9fafb; padding: 30px; text-align: center; border-top: 1px solid #e5e7eb;'>
                <p style='margin: 0 0 10px; color: #6b7280; font-size: 12px;'>
                    This is an automated message, please do not reply to this email.
                </p>
                <p style='margin: 0; color: #9ca3af; font-size: 12px;'>
                    &copy; 2024 Eghatha. All rights reserved.
                </p>
            </td>
        </tr>

    </table>
</body>
</html>";
        }


        public string BuildTeamInvitationEmail(
    string fullName,
    string teamName,
    string otpCode,
    int expirationMinutes)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Team Invitation</title>
</head>

<body style='margin:0; padding:0; font-family:Segoe UI, Arial; background:#f4f4f5;'>

<table align='center' width='100%' style='max-width:600px; margin:0 auto; background:#fff; border-radius:16px; overflow:hidden;'>

<!-- HEADER -->
<tr>
<td style='background:linear-gradient(135deg,#6366f1,#8b5cf6); padding:40px; text-align:center; color:white;'>
    <h1 style='margin:0;'>🚀 Welcome to {teamName}</h1>
</td>
</tr>

<!-- BODY -->
<tr>
<td style='padding:40px;'>

<h2 style='color:#111827;'>Hello {fullName},</h2>

<p style='color:#4b5563; font-size:16px;'>
You have been invited to join <strong>{teamName}</strong>.
To start using the system, please activate your account and set your password.
</p>

<!-- OTP BOX -->
<div style='margin:30px 0; text-align:center; padding:25px; border:2px dashed #d1d5db; border-radius:12px;'>
    <p style='margin:0; color:#6b7280;'>Activation Code</p>
    <div style='font-size:42px; letter-spacing:6px; font-weight:bold; color:#4f46e5; margin-top:10px;'>
        {otpCode}
    </div>
</div>

<!-- EXPIRATION -->
<p style='background:#fef3c7; padding:10px; border-left:4px solid #f59e0b; color:#92400e;'>
⏰ This code expires in {expirationMinutes} minutes.
</p>

<!-- CTA STEPS -->
<ol style='color:#374151; line-height:1.8; margin-top:20px;'>
<li>Open the application</li>
<li>Enter the invitation code</li>
<li>Set your password</li>
<li>Start using the system</li>
</ol>

<p style='color:#6b7280; margin-top:30px; font-size:13px;'>
If you did not expect this invitation, you can ignore this email.
</p>

</td>
</tr>

<!-- FOOTER -->
<tr>
<td style='background:#f9fafb; text-align:center; padding:20px; font-size:12px; color:#9ca3af;'>
© 2024 Your System
</td>
</tr>

</table>

</body>
</html>";
        }


        public string BuildVolunteerApprovedEmail(string fullName)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Account Approved</title>
</head>
<body style='margin: 0; padding: 0; font-family: Segoe UI, Arial, sans-serif; background-color: #f4f4f5;'>
    <table align='center' border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 16px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.1);'>
        
        <!-- Header -->
        <tr>
            <td style='background: linear-gradient(135deg, #22c55e 0%, #16a34a 100%); padding: 40px 30px; text-align: center;'>
                <h1 style='color: #ffffff; margin: 0; font-size: 28px; font-weight: 600;'>
                    🎉 Eghatha
                </h1>
                <p style='color: rgba(255,255,255,0.9); margin: 10px 0 0; font-size: 16px;'>
                    Volunteer Account Approved
                </p>
            </td>
        </tr>

        <!-- Content -->
        <tr>
            <td style='padding: 40px 30px;'>
                <h2 style='color: #1f2937; margin-bottom: 20px; font-size: 24px;'>
                    Welcome, {fullName}! 👋
                </h2>

                <p style='color: #4b5563; font-size: 16px; line-height: 1.6;'>
                    We’re happy to inform you that your volunteer application has been 
                    <strong style='color: #16a34a;'>approved</strong> by our team.
                </p>

                <!-- Highlight Box -->
                <div style='background-color: #ecfdf5; border-left: 4px solid #22c55e; padding: 20px; border-radius: 8px; margin: 25px 0;'>
                    <p style='margin: 0; color: #065f46; font-size: 15px;'>
                        ✅ Your account is now fully activated.<br/>
                        ✅ You can log in using your email and password.<br/>
                        ✅ You can start exploring and accepting missions.
                    </p>
                </div>

                <!-- Instructions -->
                <div style='margin-top: 30px;'>
                    <p style='color: #4b5563; font-size: 14px;'>
                        <strong>What’s next?</strong>
                    </p>
                    <ol style='color: #4b5563; font-size: 14px; line-height: 1.8; padding-left: 20px;'>
                        <li>Log in to your Eghatha account</li>
                        <li>Complete your profile (if needed)</li>
                        <li>Browse available missions</li>
                        <li>Start helping others 🚀</li>
                    </ol>
                </div>

                <!-- CTA -->
                <div style='text-align: center; margin: 30px 0;'>
                    <a href='#' style='background-color: #4f46e5; color: #ffffff; padding: 14px 28px; text-decoration: none; border-radius: 8px; font-weight: 600; display: inline-block;'>
                        Login to Your Account
                    </a>
                </div>

                <!-- Note -->
                <p style='color: #6b7280; font-size: 13px; margin-top: 20px;'>
                    If you did not request this account, please contact our support team immediately.
                </p>
            </td>
        </tr>

        <!-- Footer -->
        <tr>
            <td style='background-color: #f9fafb; padding: 30px; text-align: center; border-top: 1px solid #e5e7eb;'>
                <p style='margin: 0 0 10px; color: #6b7280; font-size: 12px;'>
                    This is an automated message, please do not reply.
                </p>
                <p style='margin: 0; color: #9ca3af; font-size: 12px;'>
                    &copy; 2024 Eghatha. All rights reserved.
                </p>
            </td>
        </tr>

    </table>
</body>
</html>";
        }

        public string BuildVolunteerRejectedEmail(string fullName, string? reason)
        {
            var reasonHtml = string.IsNullOrWhiteSpace(reason)
                ? ""
                : $@"
        <div style='background-color: #fef2f2; border-left: 4px solid #ef4444; padding: 15px 20px; border-radius: 8px; margin: 20px 0;'>
            <p style='margin: 0; color: #7f1d1d; font-size: 14px;'>
                <strong>Reason:</strong> {reason}
            </p>
        </div>";

            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Application Update</title>
</head>

<body style='margin:0; padding:0; font-family: Segoe UI, Arial, sans-serif; background-color:#f4f4f5;'>

<table align='center' width='100%' style='max-width:600px; margin:auto; background:#ffffff; border-radius:16px; overflow:hidden; box-shadow:0 4px 6px rgba(0,0,0,0.1);'>

    <!-- Header -->
    <tr>
        <td style='background: linear-gradient(135deg, #ef4444, #dc2626); padding:40px; text-align:center;'>
            <h1 style='color:#fff; margin:0;'>Eghatha</h1>
            <p style='color:rgba(255,255,255,0.85); margin-top:10px;'>
                Volunteer Application Update
            </p>
        </td>
    </tr>

    <!-- Body -->
    <tr>
        <td style='padding:40px;'>

            <h2 style='color:#1f2937;'>Hello {fullName},</h2>

            <p style='color:#4b5563; font-size:15px; line-height:1.6;'>
                Thank you for applying to become a volunteer with Eghatha.
            </p>

            <p style='color:#4b5563; font-size:15px; line-height:1.6;'>
                After careful review, we regret to inform you that your application was 
                <strong style='color:#dc2626;'>not approved</strong> at this time.
            </p>

            {reasonHtml}

            <div style='background:#f9fafb; padding:15px; border-radius:8px; margin-top:20px;'>
                <p style='margin:0; color:#374151; font-size:14px;'>
                    You are welcome to improve your profile and apply again in the future.
                </p>
            </div>

            <p style='color:#6b7280; font-size:13px; margin-top:20px;'>
                If you believe this was a mistake, please contact our support team.
            </p>

        </td>
    </tr>

    <!-- Footer -->
    <tr>
        <td style='background:#f9fafb; padding:20px; text-align:center; border-top:1px solid #e5e7eb;'>
            <p style='font-size:12px; color:#9ca3af; margin:0;'>
                &copy; 2024 Eghatha. All rights reserved.
            </p>
        </td>
    </tr>

</table>

</body>
</html>";
        }

    }
}

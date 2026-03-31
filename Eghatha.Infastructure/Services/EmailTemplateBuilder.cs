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
    }
}

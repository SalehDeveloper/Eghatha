using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Common.Services;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Authentication.Commands.RequestPasswordReset
{
    public class RequestPasswordResetCommandHandler : IRequestHandler<RequestPasswordResetCommand, ErrorOr<string>>
    {
        private readonly IIdentityService _identityService;
 
        private readonly IEmailService _emailService;
       
        private readonly OtpSettings _otpSettings;
       
        private readonly IOtpService _otpService;

        public RequestPasswordResetCommandHandler(
            IIdentityService identityService,
            IEmailService emailService,
            IOptions<OtpSettings> otpSettings,
            IOtpService otpService)
        {
            _identityService = identityService;
            _emailService = emailService;
            _otpSettings = otpSettings.Value;
            _otpService = otpService;
        }

        public async Task<ErrorOr<string>> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityService.GetIdentityUserByEmailAsync(request.Email, cancellationToken);

            if (user.IsError)
                return user.Errors;


            var otp = await _otpService.RequestAsync(OtpType.ResetPassword, user.Value.Email, TimeSpan.FromMinutes(_otpSettings.PasswordResetExpirationMinutes));

            if (otp.IsError) return otp.Errors;

            await _emailService.SendPasswordResetEmailAsync(
                request.Email,
                otp.Value,
               _otpSettings.PasswordResetExpirationMinutes
            );

            return $"We've sent a code to your email address. Please check your inbox to reset your password.";
        }
    }
}

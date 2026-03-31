using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Errors;
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

namespace Eghatha.Application.Features.Authentication.Commands.ResendEmailConfirmationCode
{
    public class ResendEmailConfirmationCodeCommandHandler : IRequestHandler<ResendEmailConfirmationCodeCommand, ErrorOr<string>>
    {
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;
        private readonly OtpSettings _otpSettings;
        private readonly IOtpService _otpService;

        public ResendEmailConfirmationCodeCommandHandler(
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

        public async Task<ErrorOr<string>> Handle(ResendEmailConfirmationCodeCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityService.GetIdentityUserByEmailAsync(request.Email, cancellationToken);

            if (user.IsError)
                return user.Errors;

            if (user.Value.IsEmailConfirmed)
                return ApplicationErrors.EmailAlreadyConfirmed;

            var otp = await _otpService.RequestAsync(OtpType.ConfirmEmail, request.Email, TimeSpan.FromMinutes(_otpSettings.EmailExpirationMinutes));

            if (otp.IsError) return otp.Errors;

            await _emailService.SendConfirmEmailAsync(
                request.Email,
                otp.Value,
               _otpSettings.EmailExpirationMinutes
            );

            return $"We've sent a code to your email address. Please check your inbox to confirm your email.";
        }
    }
}

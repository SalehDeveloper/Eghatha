using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Common.Services;
using Eghatha.Domain.Volunteers.Events;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Volunteers.EventHandlers
{
    public class VolunteerCreatedEventHandler : INotificationHandler<VolunteerCreated>
    {
        private readonly  IIdentityService _identityService;
        private readonly IEmailService  _emailService;
        private readonly IOtpService _otpService;
        private readonly OtpSettings _otpSettings;

        public VolunteerCreatedEventHandler(IIdentityService identityService, IEmailService emailService, IOtpService otpService,IOptions<OtpSettings> otpSettings)
        {
            _identityService = identityService;
            _emailService = emailService;
            _otpService = otpService;
            _otpSettings = otpSettings.Value;
        }

        public async Task Handle(VolunteerCreated notification, CancellationToken cancellationToken)
        {
            var user = await _identityService.GetUserDetailsByIdAsync(notification.UserId, cancellationToken);

            if (user.IsError) return;

            var otp = await _otpService.RequestAsync(Common.Models.OtpType.ConfirmEmail, user.Value.Email, TimeSpan.FromMinutes(_otpSettings.EmailExpirationMinutes));

            await _emailService.SendConfirmEmailAsync(user.Value.Email, otp.Value, _otpSettings.EmailExpirationMinutes);



        }
    }
}

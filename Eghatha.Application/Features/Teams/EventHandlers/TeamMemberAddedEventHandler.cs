using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Common.Services;
using Eghatha.Domain.Teams.Events;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.EventHandlers
{
    public class TeamMemberAddedEventHandler : INotificationHandler<TeamMemeberAddedEvent>
    {
        private readonly IEmailService _emailService;
        private readonly IIdentityService _identityService;
        private readonly IOtpService _otpService;
        private readonly OtpSettings _otpSettings;

        public TeamMemberAddedEventHandler(IEmailService emailService, IIdentityService identityService, IOtpService otpService , IOptions<OtpSettings> otpSettings)
        {
            _emailService = emailService;
            _identityService = identityService;
            _otpService = otpService;
            _otpSettings = otpSettings.Value;
        }

        public async Task Handle(TeamMemeberAddedEvent notification, CancellationToken cancellationToken)
        {
            var user = await _identityService.GetUserDetailsByIdAsync(notification.UserId , cancellationToken);

            if (user.IsError) return;

            var otp = await _otpService.RequestAsync(Common.Models.OtpType.ResetPassword, user.Value.Email, TimeSpan.FromMinutes(_otpSettings.PasswordResetExpirationMinutes));


            await _emailService.SendTeamInvitationEmailAsync(
                user.Value.Email,
                $"{user.Value.FirstName} {user.Value.LastName}",
                notification.TeamName,
                otp.Value,
                _otpSettings.PasswordResetExpirationMinutes);


        }
    }
}

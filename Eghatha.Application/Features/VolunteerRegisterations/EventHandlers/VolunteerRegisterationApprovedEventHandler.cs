using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Services;
using Eghatha.Domain.VolunteerRegisterations.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.VolunteerRegisterations.EventHandlers
{
    public class VolunteerRegisterationApprovedEventHandler : INotificationHandler<VolunteerRegisterationApproved>
    {
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;
        private readonly IVolunteerRepository _volunteerRepository;

        public VolunteerRegisterationApprovedEventHandler(IIdentityService identityService, IEmailService emailService, IVolunteerRepository volunteerRepository)
        {
            _identityService = identityService;
            _emailService = emailService;
            _volunteerRepository = volunteerRepository;
        }

        public async Task Handle(VolunteerRegisterationApproved notification, CancellationToken cancellationToken)
        {
            var volunteer = await _volunteerRepository.GetByIdAsync(notification.VolunteerId , cancellationToken);
            var user = await _identityService.GetUserDetailsByIdAsync(volunteer.UserId, cancellationToken);

            await _emailService.SendVolunteerApprovedEmailAsync(user.Value.Email, $"{user.Value.FirstName} {user.Value.LastName}");
        }
    }
}

using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Services;
using Eghatha.Domain.Disasters.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.EventHandlers
{
    internal class VolunteersAssignedToDisasterEventHandler : INotificationHandler<VolunteersAssignedToDisaster>
    {
        private readonly IEmailService _emailService;
        private readonly IVolunteerRepository _volunteerRepository;


        public VolunteersAssignedToDisasterEventHandler(IEmailService emailService, IVolunteerRepository volunteerRepository)
        {
            _emailService = emailService;
            _volunteerRepository = volunteerRepository;
        }

        public async Task Handle(VolunteersAssignedToDisaster notification, CancellationToken cancellationToken)
        {
            var volunteers = await _volunteerRepository.GetVolunteersDetailsByIdsAsync(notification.VolunteerIds, cancellationToken); 
            
            foreach(var vol in volunteers)
            {
                await _emailService.SendDisasterAssignmentEmailAsync(
                    vol.Email,
                    vol.FullName,
                    notification.Location.Latitude,
                    notification.Location.Longitude,
                    notification.Title,
                    notification.DisasterType,
                    notification.City,
                    notification.Province,
                    notification.StartTime,
                    notification.Description
                    );
            }
            
        }
    }
}

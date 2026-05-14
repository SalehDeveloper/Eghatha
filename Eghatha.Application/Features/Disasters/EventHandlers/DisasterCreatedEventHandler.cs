using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Disaster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.EventHandlers
{
    public class DisasterCreatedEventHandler : INotificationHandler<DisasterCreated>
    {
        private readonly IAdminNotifier _adminNotifier;

        public DisasterCreatedEventHandler(IAdminNotifier adminNotifier)
        {
            _adminNotifier = adminNotifier;
        }

        public async Task Handle(DisasterCreated notification, CancellationToken cancellationToken)
        {
            await _adminNotifier.NotifyNewDisasterReported(
                notification.Id,
                $"a new diaster of type {notification.Type} has been reported in {notification.Province}, {notification.City} at {notification.OccuredAt}" , 
                notification.Latitude ,
                notification.Longitude , 
                $"/disasters/{notification.Id}" , 
                notification.OccuredAt , 
                cancellationToken);
        }
    }
}

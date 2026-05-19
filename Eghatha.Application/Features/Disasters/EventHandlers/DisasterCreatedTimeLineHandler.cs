using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Disaster;
using Eghatha.Domain.Disasters;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.EventHandlers
{
    public sealed class DisasterCreatedTimeLineHandler : INotificationHandler<DisasterCreated>
    {
        private readonly IDisasterTimeLineRepository _repository;

        public DisasterCreatedTimeLineHandler(IDisasterTimeLineRepository repository)
        {
            _repository = repository;
        }

        public Task Handle(DisasterCreated notification, CancellationToken cancellationToken)
        {
            var timeLine = DisasterTimeLineEvent.Create(
                notification.Id,
               DisasterTimelineEventTypes.Created,
               $"Disaster created in {notification.City}, {notification.Province}",
                notification.OccuredAt);
            return _repository.AddAsync(timeLine, cancellationToken);
        }
    }
}

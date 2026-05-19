using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Disaster;
using Eghatha.Domain.Disasters;
using MediatR;

namespace Eghatha.Application.Features.Disasters.EventHandlers
{
    public sealed class DisasterCancelledTimeLineHandler
    : INotificationHandler<DisasterCancelled>
    {
        private readonly IDisasterTimeLineRepository _repository;

        public DisasterCancelledTimeLineHandler(IDisasterTimeLineRepository repository)
        {
            _repository = repository;
        }

        public Task Handle(DisasterCancelled notification, CancellationToken cancellationToken)
        {
            var timeline = DisasterTimeLineEvent.Create(
                notification.Id,
                DisasterTimelineEventTypes.Cancelled,
                $"Disaster was cancelled at {notification.CancelledAt:u}",
                notification.CancelledAt);

            return _repository.AddAsync(timeline, cancellationToken);
        }
    }

}

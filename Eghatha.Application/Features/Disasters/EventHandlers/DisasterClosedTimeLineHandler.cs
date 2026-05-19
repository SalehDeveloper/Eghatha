using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Disaster;
using Eghatha.Domain.Disasters;
using MediatR;

namespace Eghatha.Application.Features.Disasters.EventHandlers
{
    public sealed class DisasterClosedTimeLineHandler
    : INotificationHandler<DisasterClosed>
    {
        private readonly IDisasterTimeLineRepository _repository;
        private readonly TimeProvider _timeProvider;


        public DisasterClosedTimeLineHandler(IDisasterTimeLineRepository repository, TimeProvider timeProvider)
        {
            _repository = repository;
            _timeProvider = timeProvider;
        }

        public Task Handle(DisasterClosed notification, CancellationToken cancellationToken)
        {
            var timeline = DisasterTimeLineEvent.Create(
                notification.Id,
                DisasterTimelineEventTypes.Closed,
                "Disaster lifecycle has been fully closed",
                _timeProvider.GetUtcNow());

            return _repository.AddAsync(timeline, cancellationToken);
        }
    }
}

using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Disaster;
using Eghatha.Domain.Disasters;
using MediatR;

namespace Eghatha.Application.Features.Disasters.EventHandlers
{
    public sealed class DisasterResponseStartedTimeLineHandler
    : INotificationHandler<DisasterResponseStarted>
    {
        private readonly IDisasterTimeLineRepository _repository;

        public DisasterResponseStartedTimeLineHandler(IDisasterTimeLineRepository repository)
        {
            _repository = repository;
        }

        public Task Handle(DisasterResponseStarted notification, CancellationToken cancellationToken)
        {
            var timeline = DisasterTimeLineEvent.Create(
                notification.Id,
                DisasterTimelineEventTypes.ResponseStarted,
                $"Disaster response started at {notification.ResponseStartedAt:u}",
                notification.ResponseStartedAt);

            return _repository.AddAsync(timeline, cancellationToken);
        }
    }
}

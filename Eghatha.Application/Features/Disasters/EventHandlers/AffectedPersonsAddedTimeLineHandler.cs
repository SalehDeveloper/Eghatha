using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Disaster;
using Eghatha.Domain.Disasters;
using MediatR;

namespace Eghatha.Application.Features.Disasters.EventHandlers
{
    public sealed class AffectedPersonsAddedTimeLineHandler
    : INotificationHandler<AffectedPersonsAdded>
    {
        private readonly IDisasterTimeLineRepository _repository;
        private readonly TimeProvider _timeProvider;

        public AffectedPersonsAddedTimeLineHandler(IDisasterTimeLineRepository repository, TimeProvider timeProvider)
        {
            _repository = repository;
            _timeProvider = timeProvider;
        }

        public Task Handle(AffectedPersonsAdded notification, CancellationToken cancellationToken)
        {
            var timeline = DisasterTimeLineEvent.Create(
                notification.DisasterId,
                DisasterTimelineEventTypes.AffectedPersonAdded,
                $"{notification.PersonIds.Count} affected persons recorded",
                _timeProvider.GetUtcNow());

            return _repository.AddAsync(timeline, cancellationToken);
        }
    }

}

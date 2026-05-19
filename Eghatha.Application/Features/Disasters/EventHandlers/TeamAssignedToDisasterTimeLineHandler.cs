using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Disasters;
using Eghatha.Domain.Disasters.Events;
using MediatR;

namespace Eghatha.Application.Features.Disasters.EventHandlers
{
    public sealed class TeamAssignedToDisasterTimeLineHandler
    : INotificationHandler<TeamAssignedToDisasterEvent>
    {
        private readonly IDisasterTimeLineRepository _repository;
        private readonly TimeProvider _timeProvider;


        public TeamAssignedToDisasterTimeLineHandler(IDisasterTimeLineRepository repository, TimeProvider timeProvider)
        {
            _repository = repository;
            _timeProvider = timeProvider;
        }

        public Task Handle(TeamAssignedToDisasterEvent notification, CancellationToken cancellationToken)
        {
            var timeline = DisasterTimeLineEvent.Create(
                notification.DisasterId,
                 DisasterTimelineEventTypes.TeamAssigned,
                $"Team {notification.TeamId} assigned to disaster in {notification.City}",
                _timeProvider.GetUtcNow());

            return _repository.AddAsync(timeline, cancellationToken);
        }
    }
}

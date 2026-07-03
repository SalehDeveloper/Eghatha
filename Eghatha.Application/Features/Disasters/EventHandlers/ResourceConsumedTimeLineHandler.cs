using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Disasters;
using Eghatha.Domain.Disasters.Events;
using MediatR;

namespace Eghatha.Application.Features.Disasters.EventHandlers
{
    public sealed class ResourceConsumedTimeLineHandler
    : INotificationHandler<ResourceConsumed>
    {
        private readonly IDisasterTimeLineRepository _repository;
        private readonly ITeamRepository _teamRepository; 
        private readonly TimeProvider _timeProvider;

        public ResourceConsumedTimeLineHandler(
            IDisasterTimeLineRepository repository, TimeProvider timeProvider, ITeamRepository teamRepository)
        {
            _repository = repository;
            _timeProvider = timeProvider;
            _teamRepository = teamRepository;
        }

        public async Task Handle(ResourceConsumed notification, CancellationToken cancellationToken)
        {
            var team = await _teamRepository.GetByIdAsync(notification.TeamId, cancellationToken);
          
            var timeline = DisasterTimeLineEvent.Create(
                notification.DisasterId,
                DisasterTimelineEventTypes.ResourceUpdated,
                $"Consumed {notification.Quantity} units from resource {notification.Type} Belongs To Team{team.Name}",
                _timeProvider.GetUtcNow());

            await _repository.AddAsync(timeline, cancellationToken);
        }
    }


}

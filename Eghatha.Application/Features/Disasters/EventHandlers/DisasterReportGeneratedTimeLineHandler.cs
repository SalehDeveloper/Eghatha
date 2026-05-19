using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Disaster;
using Eghatha.Domain.Disasters;
using MediatR;

namespace Eghatha.Application.Features.Disasters.EventHandlers
{
    public sealed class DisasterReportGeneratedTimeLineHandler
    : INotificationHandler<DisasterReportGenerated>
    {
        private readonly IDisasterTimeLineRepository _repository;
        private readonly TimeProvider _timeProvider;

        public DisasterReportGeneratedTimeLineHandler(IDisasterTimeLineRepository repository, TimeProvider timeProvider)
        {
            _repository = repository;
            _timeProvider = timeProvider;
        }

        public Task Handle(DisasterReportGenerated notification, CancellationToken cancellationToken)
        {
            var timeline = DisasterTimeLineEvent.Create(
                notification.DisasterId,
                DisasterTimelineEventTypes.ReportGenerated,
                $"Final disaster report has been generated {notification.ReportId} ",
                _timeProvider.GetUtcNow());

            return _repository.AddAsync(timeline, cancellationToken);
        }
    }

}

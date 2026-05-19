using Eghatha.Domain.Abstractions;

namespace Eghatha.Domain.Disaster
{
    public sealed class DisasterReportGenerated : DomainEvent
    {
        public Guid DisasterId { get; }
        public Guid ReportId { get; }


        public DisasterReportGenerated(Guid disasterId, Guid reportId)
        {
            DisasterId = disasterId;
            ReportId = reportId;
        }
    }
}
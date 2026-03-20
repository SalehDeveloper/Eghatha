using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Disasters;

namespace Eghatha.Domain.Disaster
{
    public sealed class DisasterCancelled : DomainEvent
    { 
        public Guid Id { get; set;  }
        public DisasterStatus Status {get; set; }
        public DateTimeOffset CancelledAt { get; set;}

        public DisasterCancelled(Guid id ,  DisasterStatus status ,  DateTimeOffset cancelledAt) 
        {
            Id = id;    
            Status = status;    
            CancelledAt = cancelledAt;
        }
    }
}
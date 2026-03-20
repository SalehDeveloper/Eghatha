using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Disasters;

namespace Eghatha.Domain.Disaster
{
    public sealed class DisasterResolved : DomainEvent
    {
        public Guid Id { get; set;} 
        public DisasterStatus  Status  { get; set;}   
        public DateTimeOffset ResolvedAt { get; set;}

      
        public DisasterResolved(Guid id , DisasterStatus status ,  DateTimeOffset resolvedAt) 
        {
            Id = id;    
            Status = status;
            ResolvedAt = resolvedAt;
        }
    }
}
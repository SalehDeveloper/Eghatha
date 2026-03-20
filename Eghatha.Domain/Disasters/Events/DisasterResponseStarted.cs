
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Disasters;

namespace Eghatha.Domain.Disaster
{
    public sealed class DisasterResponseStarted :DomainEvent
    {
        public Guid Id { get; set;}

        public DisasterStatus  Status  { get; set;}    

        public DateTimeOffset ResponseStartedAt { get; set;}

          
          public DisasterResponseStarted( Guid id , DisasterStatus status , DateTimeOffset responseStartedAt)
          {
            Id = id;
            Status = status;  
            ResponseStartedAt = responseStartedAt;
          }
    }
}
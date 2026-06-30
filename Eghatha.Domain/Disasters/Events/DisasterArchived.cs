using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Disasters;

namespace Eghatha.Domain.Disaster
{
    public sealed class DisasterArchived : DomainEvent
    {
        public Guid Id { get; set; }

        public DisasterStatus Status { get; set; }


        public DisasterArchived(Guid id, DisasterStatus status)
        {
            Id = id;
            Status = status;
        }
    }
}
using Eghatha.Domain.Abstractions;

namespace Eghatha.Domain.Disaster
{
    public sealed class AffectedPersonsAdded : DomainEvent
    {
        public Guid DisasterId { get; }
        public List<Guid> PersonIds { get; }

        public AffectedPersonsAdded(Guid disasterId, List<Guid> personIds)
        {
            DisasterId = disasterId;
            PersonIds = personIds;
        }
    }
}
using Eghatha.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Disasters
{
    public sealed class DisasterTimeLineEvent : AuditableEntity
    {
        public Guid DisasterId { get; private set; }

        public string EventType { get; private set; }

        public string Description { get; private set; }

        public DateTimeOffset OccurredAt { get; private set; }

        private DisasterTimeLineEvent()
        {
            
        }

        private DisasterTimeLineEvent(
            Guid id,
            Guid disasterId,
            string eventType,
            string description,
            DateTimeOffset occurredAt)
            : base(id)
        {
            DisasterId = disasterId;
            EventType = eventType;
            Description = description;
            OccurredAt = occurredAt;
        }

        public static DisasterTimeLineEvent Create(
            Guid disasterId,
            string eventType,
            string description,
            DateTimeOffset occurredAt)
        {
            return new DisasterTimeLineEvent(
                Guid.NewGuid(),
                disasterId,
                eventType,
                description,
                occurredAt);
        }
    }
}

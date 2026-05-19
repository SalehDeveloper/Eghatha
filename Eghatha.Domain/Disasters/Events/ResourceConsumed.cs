using Eghatha.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Disasters.Events
{
    public class ResourceConsumed : DomainEvent
    {
        public ResourceConsumed(Guid disasterId, Guid resourceId, int quantity, Guid teamId)
        {
            DisasterId = disasterId;
            ResourceId = resourceId;
            Quantity = quantity;
            TeamId = teamId;
        }

        public Guid DisasterId { get; }

        public Guid ResourceId { get; }

        public Guid TeamId { get; }

        public int Quantity { get; }



    }
}

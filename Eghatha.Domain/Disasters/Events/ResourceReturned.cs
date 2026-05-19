using Eghatha.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Disasters.Events
{
    public class ResourceReturned : DomainEvent
    {
        public ResourceReturned(Guid disasterId, Guid resourceId, Guid teamId, int quantity)
        {
            DisasterId = disasterId;
            ResourceId = resourceId;
            TeamId = teamId;
            Quantity = quantity;
        }

        public Guid DisasterId { get; }

        public Guid ResourceId { get; }

        public Guid TeamId {  get; }

        public int Quantity { get; }    


    }
}

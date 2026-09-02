using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Teams.TeamResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Disasters.Events
{
    public class ResourceDamaged:DomainEvent
    {
        public ResourceDamaged(Guid disasterId, Guid resourceId, Guid teamId, int quantity, ResourceType resourceType)
        {
            DisasterId = disasterId;
            ResourceId = resourceId;
            TeamId = teamId;
            Quantity = quantity;
            ResourceType = resourceType;
        }

        public Guid DisasterId { get; }

        public Guid ResourceId { get; }

        public Guid TeamId { get; }

        public int Quantity { get; }

        public ResourceType ResourceType { get; }
    }
}

using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Shared.Errors;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Teams.Resources
{
    public sealed class Resource : AuditableEntity
    {
        public Guid TeamId { get; private set; }
        public ResourceType Type { get; private set; }

        public int Quantity { get; private set; }

        public ResourceStatus Status { get; private set; }

        private Resource()
        {
        }

        private Resource(
            Guid id,
            Guid teamId,
            ResourceType type,
            int quantity
            ) : base(id)
        {
            TeamId = teamId;
            Type = type;
            Quantity = quantity;
            Status = ResourceStatus.Available;

        }

        public static ErrorOr <Resource> Create(
            Guid id , 
            Guid teamId,
            ResourceType type,
            int quantity
            )
        {
            if (id == Guid.Empty )
                return DomainErrors.IdMustBeProvided(nameof(Resource));

            if (teamId == Guid.Empty)
                return DomainErrors.IdMustBeProvided(nameof(Team));

            if (type is null )
                return ResourceErrors.ResourceTypeRequired;

            if (!ResourceType.List.Contains(type))
                return ResourceErrors.InvalidResourceType;

            if (quantity <= 0)
                return ResourceErrors.QuantityShouldBeGreaterThanZero;
          
            


            return new Resource(
                id,
                teamId,
                type,
                quantity
                );
        }

        public ErrorOr<Updated> IncreaseQuantity(int amount)
        {
            if (amount <= 0)
                return ResourceErrors.QuantityShouldBeGreaterThanZero;

            Quantity += amount;

            return Result.Updated;
        }

        public ErrorOr<Updated> DecreaseQuantity(int amount)
        {
            if (amount <= 0)
                return ResourceErrors.QuantityShouldBeGreaterThanZero;

            if (Quantity < amount)
                return ResourceErrors.NotEnoughResources;

            Quantity -= amount;

            return Result.Updated;
        }

        public ErrorOr<Updated> UpdateStatus(ResourceStatus newStatus)
        {
            if (newStatus is null)
                return ResourceErrors.StatusRequired;

            Status = newStatus;

            return Result.Updated;
        }

    }
}

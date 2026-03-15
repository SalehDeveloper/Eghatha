using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Shared.Errors;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Disasters.DisasterResources
{
    public sealed class DisasterResource : AuditableEntity
    { 
        public Guid DisasterId { get; private set; }
        public Guid ResourceId { get; private set; }    

        public Guid TeamId { get; private set; }
        public int QuantitySent { get; private set; }
        public int QuantityConsumed { get; private set; }
        public int QuantityReturned { get; private set; }
        public int QuantityDamaged { get; private set; }

        public DateTimeOffset AssignedAt { get; private set; }

        public string? Notes { get; private set; }

        public int RemainingQuantity => QuantitySent - QuantityConsumed - QuantityReturned - QuantityDamaged;

        private DisasterResource()
        {
            
        }

        public DisasterResource(
            Guid id,
            Guid disasterId,
            Guid resourceId,
            Guid teamId , 
            int quantitySent,
            DateTimeOffset assignedAt,
            string? notes)
            : base(id)
        {
            DisasterId = disasterId;
            ResourceId = resourceId;
            QuantitySent = quantitySent;
            AssignedAt = assignedAt;
            Notes = notes;
            TeamId = teamId;
        }

        public static ErrorOr<DisasterResource> Create(
            Guid id ,
            Guid disasterId,
            Guid resourceId,
            Guid teamId ,
            int quantitySent,
            DateTimeOffset assignedAt ,
            string? notes )
        {
            if (id == Guid.Empty)
                return DomainErrors.IdMustBeProvided("DisasterResource");

            if (disasterId == Guid.Empty)
                return DomainErrors.IdMustBeProvided("Disaster");

            if (resourceId == Guid.Empty)
                return DomainErrors.IdMustBeProvided("Resource");

            if (teamId == Guid.Empty)
                return DomainErrors.IdMustBeProvided("Team");

            if (quantitySent <= 0)
                return Error.Validation(
                    code: "DisasterResources.QuantitySentShouldBeGreaetrThanZero",
                    description: "quantity sent should be greater than zero");

            return new DisasterResource(id, disasterId, resourceId,teamId, quantitySent, assignedAt , notes);
        }

        public ErrorOr<Updated> IncreaseQuantity(int quantity)
        {
            if (quantity <= 0)
                return DisasterErrors.ResourceQuantityshouldBeGreaterThanZero;
            
            QuantitySent += quantity;
            return Result.Updated;
        }
        public ErrorOr<Updated> Consume(int quantity)
        {
            if (quantity <= 0)
                return DisasterErrors.ResourceQuantityshouldBeGreaterThanZero;

            if (quantity > RemainingQuantity)
                return DisasterResourceErrors.ResourceConsumptionExceedsSent;

            QuantityConsumed += quantity;

            return Result.Updated;
        }

        public ErrorOr<Updated> Return(int quantity)
        {
            if (quantity <= 0)
                return DisasterErrors.ResourceQuantityshouldBeGreaterThanZero;

            if (quantity > RemainingQuantity)
                return DisasterResourceErrors.InvalidReturnQuantity;

            QuantityReturned += quantity;

            return Result.Updated;
        }

        public ErrorOr<Updated> MarkDamaged(int quantity)
        {
            if (quantity <= 0)
                return DisasterErrors.ResourceQuantityshouldBeGreaterThanZero;

            if (quantity > RemainingQuantity)
                return DisasterResourceErrors.InvalidDamagedQuantity;

            QuantityDamaged += quantity;

            return Result.Updated;
        }



    }
}

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
        public int QuantitySent { get; private set; }
        public DateTimeOffset AssignedAt { get; private set; }

        public string? Notes { get; private set; }


        private DisasterResource()
        {
            
        }

        public DisasterResource(
            Guid id,
            Guid disasterId,
            Guid resourceId,
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
        }

        public static ErrorOr<DisasterResource> Create(
            Guid id ,
            Guid disasterId,
            Guid resourceId,
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

            if (quantitySent <= 0)
                return Error.Validation(
                    code: "DisasterResources.QuantitySentShouldBeGreaetrThanZero",
                    description: "quantity sent should be greater than zero");

            return new DisasterResource(id, disasterId, resourceId, quantitySent, assignedAt , notes);
        }

        public ErrorOr<Updated> IncreaseQuantity(int quantity)
        {

            if (quantity <= 0)
                return DisasterErrors.ResourceQuantityshouldBeGreaterThanZero;

            QuantitySent += quantity;

            return Result.Updated;
            
        }
    }
}

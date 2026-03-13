using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Shared.Errors;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Volunteers.Equipments
{
    public sealed class Equipment : AuditableEntity
    {
        public string Name { get; private set; }
        public EquipmentCategory Category { get; private set; }

        public int Quantity { get; private set; }   

        public EquipmentStatus Status { get; private set; }


        private Equipment(
            Guid id,
            string name,
            EquipmentCategory category,
            int quantity)
            :base(id)
        {
            Name = name;
            Category = category;
            Quantity = quantity;
            Status = EquipmentStatus.Valid;
        }

        public static ErrorOr<Equipment> Create(
            Guid id,
            string name,
            EquipmentCategory category,
            int quantity)
        {
            if (id == Guid.Empty)
                return DomainErrors.IdMustBeProvided;

            if (string.IsNullOrWhiteSpace(name))
                return EquipmentErrors.NameRequired;

            if (!EquipmentCategory.TryFromName(category.Name, out _))
                return EquipmentErrors.UnSupportedCategory;

            if (quantity <= 0)
                return EquipmentErrors.QuantityShouldBeGreaterThanZero;

            return new Equipment(id, name, category, quantity);

             
        }

        public ErrorOr<Updated> Update(
            string? name =null,
            EquipmentCategory? category=null,
            EquipmentStatus? status = null,
            int? quantity = null)
        {
            
            if (name is not null && string.IsNullOrWhiteSpace(name))
                return EquipmentErrors.NameRequired;

            if (category is not null && !EquipmentCategory.TryFromName(category.Name, out _))
                return EquipmentErrors.UnSupportedCategory;

            if (status is not null && !VolunteerStatus.TryFromName(status.Name, out _))
                return EquipmentErrors.InvalidStatus;

            if ( quantity is not null && quantity <= 0)
                return EquipmentErrors.QuantityShouldBeGreaterThanZero;

            Name = name ?? Name;
            Status = status?? Status;
            Category = category ?? Category;
            Quantity = quantity ?? Quantity;

            return Result.Updated;
        }


        public ErrorOr<Updated> ChangeQuantity(int quantity)
        {
            if (quantity<= 0)
                return EquipmentErrors.QuantityShouldBeGreaterThanZero;

            Quantity = quantity;

            return Result.Updated;
        }

        public ErrorOr<Updated> UpdateStatus( EquipmentStatus status)
        {
            if (!EquipmentStatus.TryFromName(status.Name, out _))
                return EquipmentErrors.InvalidStatus;
           
            Status = status;
            return Result.Updated;
        }




    }
}

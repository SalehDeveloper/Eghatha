using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Volunteers.Equipments
{
    public class EquipmentStatus : SmartEnum<EquipmentStatus>
    {
        public readonly static EquipmentStatus Valid = new(nameof(Valid), 1);
        public readonly static EquipmentStatus InValid = new(nameof(InValid), 2);
        public EquipmentStatus(string name, int value) : base(name, value)
        {
        }
    }
}

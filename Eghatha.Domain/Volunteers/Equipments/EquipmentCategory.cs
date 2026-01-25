using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Volunteers.Equipments
{
    public class EquipmentCategory : SmartEnum<EquipmentCategory>
    {
        public static readonly EquipmentCategory Medical = new(nameof(Medical), 1);
        public static readonly EquipmentCategory Firefighting = new(nameof(Firefighting), 2);
        public static readonly EquipmentCategory Rescue = new(nameof(Rescue), 3);
        public static readonly EquipmentCategory Communication = new(nameof(Communication), 4);
        public static readonly EquipmentCategory Logistics = new(nameof(Logistics), 5);
        public static readonly EquipmentCategory ProtectiveGear = new(nameof(ProtectiveGear), 6);
        public static readonly EquipmentCategory HeavyEquipment = new(nameof(HeavyEquipment), 7);
        public static readonly EquipmentCategory WaterRescue = new(nameof(WaterRescue), 8);
        public static readonly EquipmentCategory Miscellaneous = new(nameof(Miscellaneous), 9);
        public EquipmentCategory(string name, int value) : base(name, value)
        {
        }
    }
}

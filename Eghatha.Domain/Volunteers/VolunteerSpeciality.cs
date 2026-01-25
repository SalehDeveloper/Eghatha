using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Volunteers
{
    public class VolunteerSpeciality : SmartEnum<VolunteerSpeciality>
    {
        public static readonly VolunteerSpeciality General = new(nameof(General), 1);
        public static readonly VolunteerSpeciality FirstAid = new(nameof(FirstAid), 2);
        public static readonly VolunteerSpeciality FireFighting = new(nameof(FireFighting), 3);
        public static readonly VolunteerSpeciality SearchAndRescue = new(nameof(SearchAndRescue), 4);
        public static readonly VolunteerSpeciality Medical = new(nameof(Medical), 5);
        public static readonly VolunteerSpeciality Logistics = new(nameof(Logistics), 6);
        public static readonly VolunteerSpeciality Engineering = new(nameof(Engineering), 7);
        public static readonly VolunteerSpeciality Communications = new(nameof(Communications), 8);
        public static readonly VolunteerSpeciality HeavyEquipmentOperator = new(nameof(HeavyEquipmentOperator), 9);
        public static readonly VolunteerSpeciality WaterRescue = new(nameof(WaterRescue), 10);
        public static readonly VolunteerSpeciality MountainRescue = new(nameof(MountainRescue), 11);
        public static readonly VolunteerSpeciality FieldCommander = new(nameof(FieldCommander), 12);

        public VolunteerSpeciality(string name, int value) : base(name, value)
        {
        }
    }
}

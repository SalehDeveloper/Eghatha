using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Teams
{
    public class TeamSpeciality : SmartEnum<TeamSpeciality>
    {
        public static readonly TeamSpeciality FireFightingTeam = new(nameof(FireFightingTeam), 1);
        public static readonly TeamSpeciality MedicalTeam = new(nameof(MedicalTeam), 2);
        public static readonly TeamSpeciality SearchAndRescueTeam = new(nameof(SearchAndRescueTeam), 3);
        public static readonly TeamSpeciality LogisticsTeam = new(nameof(LogisticsTeam), 4);
        public static readonly TeamSpeciality EngineeringTeam = new(nameof(EngineeringTeam), 5);
        public static readonly TeamSpeciality CommunicationsTeam = new(nameof(CommunicationsTeam), 6);
        public static readonly TeamSpeciality WaterRescueTeam = new(nameof(WaterRescueTeam), 7);
        public static readonly TeamSpeciality MountainRescueTeam =new(nameof(MountainRescueTeam), 8);
        public static readonly TeamSpeciality HazardousMaterialsTeam =   new(nameof(HazardousMaterialsTeam), 9);
        public static readonly TeamSpeciality HeavyRescueTeam =new(nameof(HeavyRescueTeam), 10);
        public static readonly TeamSpeciality EvacuationTeam =new(nameof(EvacuationTeam), 11);
        public static readonly TeamSpeciality ShelterSupportTeam =new(nameof(ShelterSupportTeam), 12);
        public static readonly TeamSpeciality RapidResponseTeam =new(nameof(RapidResponseTeam), 13);
        public static readonly TeamSpeciality GeneralSupportTeam = new(nameof(GeneralSupportTeam), 14);


        public TeamSpeciality(string name, int value) : base(name, value)
        {
        }
    }
}

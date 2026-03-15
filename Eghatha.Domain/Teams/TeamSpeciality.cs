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

        public TeamSpeciality(string name, int value) : base(name, value)
        {
        }
    }
}

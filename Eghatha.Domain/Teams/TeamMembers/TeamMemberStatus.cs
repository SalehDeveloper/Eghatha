using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Teams.TeamMembers
{
    public class TeamMemberStatus : SmartEnum<TeamMemberStatus>
    {
        public static readonly TeamMemberStatus Active = new(nameof(Active), 1);
        public static readonly TeamMemberStatus OffDuty = new(nameof(OffDuty), 2);
        public static readonly TeamMemberStatus OnMission = new(nameof(OnMission), 3);
        public static readonly TeamMemberStatus Inactive = new(nameof(Inactive), 4);
   

        public TeamMemberStatus(string name, int value) : base(name, value)
        {
        }
    }
}

using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Teams
{
    public class TeamStatus : SmartEnum<TeamStatus>
    {
        public static readonly TeamStatus Active = new(nameof(Active), 1);
        public static readonly TeamStatus Inactive = new(nameof(Inactive), 2);
        public static readonly TeamStatus OnMission = new(nameof(OnMission), 3);
        public static readonly TeamStatus OffDuty = new(nameof(OffDuty), 4);
        public TeamStatus(string name, int value) : base(name, value)
        {
        }
    }
}

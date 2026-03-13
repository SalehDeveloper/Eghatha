using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Disasters.AffectedPersons
{
    public class HealthStatus : SmartEnum<HealthStatus>
    {
        public static readonly HealthStatus Injured = new(nameof(Injured), 1);

        public static readonly HealthStatus Missing = new(nameof(Missing), 2);
        
        public static readonly HealthStatus Dead =  new(nameof(Dead), 3);

        public static readonly HealthStatus Evacuated = new(nameof(Evacuated), 4);
        public HealthStatus(string name, int value) : base(name, value)
        {
        }
    }
}

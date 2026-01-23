using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Volunteers
{
    public class VolunteerStatus : SmartEnum<VolunteerStatus>
    {
        public static readonly VolunteerStatus Available = new VolunteerStatus(nameof(Available), 1);
        public static readonly VolunteerStatus UnAvailable = new VolunteerStatus(nameof(UnAvailable), 2);
        public static readonly VolunteerStatus Busy = new VolunteerStatus(nameof(Busy), 3);
        public VolunteerStatus(string name, int value) 
            : base(name, value)
        {
        }
    }
}

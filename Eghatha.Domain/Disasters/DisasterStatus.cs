using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Disasters
{
    public class DisasterStatus : SmartEnum<DisasterStatus>
    { 
        public static readonly DisasterStatus Reported = new DisasterStatus(nameof(Reported), 1);

        public static readonly DisasterStatus InProgress = new DisasterStatus(nameof(InProgress),2);

        public static readonly DisasterStatus Resolved = new DisasterStatus(nameof(Resolved),3);

        public static readonly DisasterStatus Closed = new DisasterStatus(nameof(Closed), 4);

        public DisasterStatus(string name, int value) 
            : base(name, value)
        {
        }
    }
}

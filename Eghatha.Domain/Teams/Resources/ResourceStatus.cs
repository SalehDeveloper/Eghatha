using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Teams.Resources
{
    public class ResourceStatus : SmartEnum<ResourceStatus>
    {
        public static readonly ResourceStatus Available = new(nameof(Available), 1);
        public static readonly ResourceStatus InUse = new(nameof(InUse), 2);
        public static readonly ResourceStatus Maintenance = new(nameof(Maintenance), 3);
        public static readonly ResourceStatus Damaged = new(nameof(Damaged), 4);
        public ResourceStatus(string name, int value) : base(name, value)
        {
        }
    }
}

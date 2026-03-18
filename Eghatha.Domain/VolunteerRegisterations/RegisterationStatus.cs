using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.VolunteerRegisterations
{
    public class RegisterationStatus : SmartEnum<RegisterationStatus>
    {
        public static readonly RegisterationStatus Pending = new RegisterationStatus(nameof(Pending), 1);

        public static readonly RegisterationStatus Approved = new RegisterationStatus(nameof(Approved), 2);

        public static readonly RegisterationStatus Rejected = new RegisterationStatus(nameof(Rejected), 3);


        public RegisterationStatus(string name, int value) : base(name, value)
        {
        }
    }
}

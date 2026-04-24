using Eghatha.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.VolunteerRegisterations.Events
{
    public sealed class VolunteerRegisterationApproved : DomainEvent
    {
        public VolunteerRegisterationApproved(Guid volunteerId)
        {
            VolunteerId = volunteerId;
        }

        public Guid VolunteerId { get; set; }
    }
}

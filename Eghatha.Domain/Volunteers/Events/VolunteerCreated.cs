using Eghatha.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Volunteers.Events
{
    public class VolunteerCreated : DomainEvent
    {
        public VolunteerCreated(Guid volunteerId, Guid userId)
        {
            VolunteerId = volunteerId;
            UserId = userId;
        }

        public Guid VolunteerId { get; private set; }
        public Guid UserId { get; private set; }

    }
}

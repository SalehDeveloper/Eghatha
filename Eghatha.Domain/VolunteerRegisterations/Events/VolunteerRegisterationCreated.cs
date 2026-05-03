using Eghatha.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.VolunteerRegisterations.Events
{
    public class VolunteerRegisterationCreated : DomainEvent
    {
        public VolunteerRegisterationCreated(Guid registerationId, Guid volunteerId, DateTimeOffset createdAt)
        {
            RegisterationId = registerationId;
            VolunteerId = volunteerId;
            CreatedAt = createdAt;
        }

        public Guid RegisterationId { get; private set; }

        public Guid VolunteerId { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }


    }
}

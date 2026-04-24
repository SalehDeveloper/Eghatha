using Eghatha.Domain.Abstractions;

namespace Eghatha.Domain.VolunteerRegisterations.Events
{
    public sealed class VolunteerRegisterationRejected : DomainEvent
    {
        public VolunteerRegisterationRejected(Guid volunteerId, string reason)
        {
            VolunteerId = volunteerId;
            Reason = reason;
        }

        public Guid VolunteerId { get; set; }

        public string Reason { get; set; }  
    }
}

using Eghatha.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Disasters.Events
{
    public sealed class TeamAssignedToDisasterEvent : DomainEvent
    {
        public TeamAssignedToDisasterEvent(Guid disasterId, Guid teamId, string disasterTitle, string city)
        {
            DisasterId = disasterId;
            TeamId = teamId;
            DisasterTitle = disasterTitle;
            City = city;
            
        }

        public Guid DisasterId { get; set; }

        public Guid TeamId { get; set; }

        public string DisasterTitle { get; set; }

        public string City { get; set; }



    }
}

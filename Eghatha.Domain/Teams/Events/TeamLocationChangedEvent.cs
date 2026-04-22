using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Teams.Events
{
    public sealed class TeamLocationChangedEvent : DomainEvent
    {
        public TeamLocationChangedEvent(Guid teamId, string teamName, GeoLocation geoLocation)
        {
            TeamId = teamId;
            TeamName = teamName;
            GeoLocation = geoLocation;
        }

        public Guid TeamId { get; private set; }

        public string TeamName { get; private set; }
        public GeoLocation GeoLocation { get; private set; }



    }
}

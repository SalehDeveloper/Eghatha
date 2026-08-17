using Eghatha.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Teams.Events
{
    public sealed class TeamStatusChangedEvent : DomainEvent
    {
        public Guid TeamId { get; }
        public string TeamName { get; }
        public TeamStatus Status { get; }

        public TeamStatusChangedEvent(Guid teamId, string teamName, TeamStatus status)
        {
            TeamId = teamId;
            TeamName = teamName;
            Status = status;
        }
    }
}

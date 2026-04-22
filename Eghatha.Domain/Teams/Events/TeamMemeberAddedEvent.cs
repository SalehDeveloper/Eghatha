using Eghatha.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Teams.Events
{
    public sealed class TeamMemeberAddedEvent : DomainEvent
    {
        public TeamMemeberAddedEvent(Guid teamId,string teamName ,  Guid userId, Guid memberId)
        {
            TeamId = teamId;
            UserId = userId;
            MemberId = memberId;
            TeamName = teamName;
        }

        public  Guid TeamId { get;private set; }
        public Guid UserId { get; private set; }
        public Guid MemberId { get; private set; }
        public string TeamName { get; private set; }






    }
}

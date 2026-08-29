using Eghatha.Domain.Teams.TeamMembers;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Tests.Common.Teams
{
    /// <summary>
    /// Fluent builder that calls <see cref="TeamMember.Create"/> directly.
    /// Use this to exercise TeamMember's own validation branches; use
    /// TeamTestFactory / Team.AddMember when a test cares about the member
    /// as part of a Team aggregate instead.
    /// </summary>
    public sealed class TeamMemberBuilder
    {
        private Guid _id = Guid.NewGuid();
        private Guid _userId = Guid.NewGuid();
        private string _jobTitle = "Paramedic";
        private bool _isLeader;
        private DateTimeOffset _joinedAt = new(2026, 1, 1, 8, 0, 0, TimeSpan.Zero);

        public static TeamMemberBuilder Valid() => new();

        public TeamMemberBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public TeamMemberBuilder WithUserId(Guid userId)
        {
            _userId = userId;
            return this;
        }

        public TeamMemberBuilder WithJobTitle(string jobTitle)
        {
            _jobTitle = jobTitle;
            return this;
        }

        public TeamMemberBuilder WithIsLeader(bool isLeader)
        {
            _isLeader = isLeader;
            return this;
        }

        public TeamMemberBuilder WithJoinedAt(DateTimeOffset joinedAt)
        {
            _joinedAt = joinedAt;
            return this;
        }

        public ErrorOr<TeamMember> Build() =>
            TeamMember.Create(_id, _userId, _jobTitle, _isLeader, _joinedAt);

        public TeamMember BuildValid() => Build().Value;
    }
}

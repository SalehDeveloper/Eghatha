using Eghatha.Domain.Teams;
using Eghatha.Domain.Teams.TeamMembers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Tests.Common.Teams
{
    /// <summary>
    /// Produces Team aggregates already advanced to a given lifecycle status,
    /// or populated with members/resources. Every transition goes through the
    /// aggregate's own public methods (never reflection/internal state hacks),
    /// so these helpers stay valid as long as the state machine's happy path
    /// doesn't change.
    ///
    /// Teams returned here have no members/resources unless a test adds them
    /// (or uses CreateActiveWithMember), which is exactly what's needed for
    /// tests exercising empty-collection guards (e.g. IsReadyForMission with
    /// no members). Tests that need a specific member/resource composition on
    /// top of a given status should build their own scenario using
    /// TeamBuilder + the aggregate's methods directly.
    /// </summary>
    public static class TeamTestFactory
    {
        public static Team CreateActive() => TeamBuilder.Valid().BuildValid();

        public static Team CreateOffDuty()
        {
            var team = CreateActive();
            team.UpdateStatus(TeamStatus.OffDuty);
            return team;
        }

        public static Team CreateOnMission()
        {
            var team = CreateActive();
            team.UpdateStatus(TeamStatus.OnMission);
            return team;
        }

        public static Team CreateReturning()
        {
            var team = CreateOnMission();
            team.UpdateStatus(TeamStatus.Returning);
            return team;
        }

        public static Team CreateInactive()
        {
            var team = CreateActive();
            team.UpdateStatus(TeamStatus.Inactive);
            return team;
        }

        /// <summary>
        /// Active team with a single active member attached.
        /// </summary>
        public static Team CreateActiveWithMember(out TeamMember member, bool isLeader = false)
        {
            var team = CreateActive();
            member = team.AddMember(Guid.NewGuid(), "Paramedic", isLeader, DateTimeOffset.UtcNow).Value;
            return team;
        }
    }
}

using Eghatha.Domain.VolunteerRegisterations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Tests.Common.VolunteerRegisterations
{
    /// <summary>
    /// Produces VolunteerRegisteration aggregates already advanced to a given
    /// lifecycle status. Every transition goes through the aggregate's own
    /// public methods (never reflection/internal state hacks), so these
    /// helpers stay valid as long as the state machine's happy path doesn't
    /// change.
    /// </summary>
    public static class VolunteerRegisterationTestFactory
    {
        public static VolunteerRegisteration CreatePending() =>
            VolunteerRegisterationBuilder.Valid().BuildValid();

        public static VolunteerRegisteration CreateApproved(
            DateTimeOffset? reviewedAt = null,
            Guid? reviewedByAdminId = null)
        {
            var registeration = CreatePending();
            registeration.Approve(
                reviewedAt ?? new DateTimeOffset(2026, 1, 2, 8, 0, 0, TimeSpan.Zero),
                reviewedByAdminId ?? Guid.NewGuid());
            return registeration;
        }

        public static VolunteerRegisteration CreateRejected(
            DateTimeOffset? reviewedAt = null,
            Guid? reviewedByAdminId = null,
            string reason = "Insufficient qualifications")
        {
            var registeration = CreatePending();
            registeration.Reject(
                reviewedAt ?? new DateTimeOffset(2026, 1, 2, 8, 0, 0, TimeSpan.Zero),
                reviewedByAdminId ?? Guid.NewGuid(),
                reason);
            return registeration;
        }
    }
}

using Eghatha.Domain.VolunteerRegisterations;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Tests.Common.VolunteerRegisterations
{
    /// <summary>
    /// Fluent builder that produces a valid, Pending <see cref="VolunteerRegisteration"/>
    /// by default. Use the With* methods to override individual fields when a test
    /// needs to exercise a specific validation branch (e.g. WithVolunteerId(Guid.Empty)).
    ///
    /// NOTE: VolunteerRegisteration.Create generates its own Id internally (it isn't
    /// accepted as a parameter), so there's no WithId here — unlike DisasterBuilder /
    /// VolunteerBuilder.
    /// </summary>
    public sealed class VolunteerRegisterationBuilder
    {
        private Guid _volunteerId = Guid.NewGuid();
        private DateTimeOffset _requestedAt = new(2026, 1, 1, 8, 0, 0, TimeSpan.Zero);

        public static VolunteerRegisterationBuilder Valid() => new();

        public VolunteerRegisterationBuilder WithVolunteerId(Guid volunteerId)
        {
            _volunteerId = volunteerId;
            return this;
        }

        public VolunteerRegisterationBuilder WithRequestedAt(DateTimeOffset requestedAt)
        {
            _requestedAt = requestedAt;
            return this;
        }

        public ErrorOr<VolunteerRegisteration> Build() =>
            VolunteerRegisteration.Create(_volunteerId, _requestedAt);

        /// <summary>
        /// Builds and unwraps the result. Only use this in tests where the
        /// input is known-valid (arrange phase) — never in the tests that
        /// are actually asserting on Create's validation behavior.
        /// </summary>
        public VolunteerRegisteration BuildValid() => Build().Value;
    }
}

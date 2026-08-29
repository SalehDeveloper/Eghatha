using Eghatha.Domain.Disasters;
using Eghatha.Domain.Disasters.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Tests.Common.Disasters
{
    /// <summary>
    /// Produces Disaster aggregates already advanced to a given lifecycle
    /// status. Every transition goes through the aggregate's own public
    /// methods (never reflection/internal state hacks), so these helpers
    /// stay valid as long as the state machine's happy path doesn't change.
    ///
    /// Disasters returned here have no volunteers/resources/affected people
    /// unless a test adds them — which is exactly what's needed for the
    /// "trivially passes because the guard is over an empty collection"
    /// Close() cases. Tests that need to exercise the unevaluated-volunteer
    /// or unmanaged-resource guards should build their own scenario using
    /// DisasterBuilder + the aggregate's methods directly.
    /// </summary>
    public static class DisasterTestFactory
    {
        public static Disaster CreateReported() => DisasterBuilder.Valid().BuildValid();

        public static Disaster CreateInProgress()
        {
            var disaster = CreateReported();
            disaster.StartResponse();
            return disaster;
        }

        public static Disaster CreateResolved(DateTimeOffset? resolvedAt = null)
        {
            var disaster = CreateInProgress();
            disaster.Resolve(resolvedAt ?? new DateTimeOffset(2026, 2, 1, 8, 0, 0, TimeSpan.Zero));
            return disaster;
        }

        /// <summary>
        /// Closed with no volunteers/resources, so the close guards pass trivially.
        /// </summary>
        public static Disaster CreateClosed()
        {
            var disaster = CreateResolved();
            disaster.Close();
            return disaster;
        }

        public static Disaster CreateClosedWithReport(out Report report)
        {
            var disaster = CreateClosed();
            report = ReportBuilder.Valid().WithDisasterId(disaster.Id).BuildValid();
            disaster.AddReport(report);
            return disaster;
        }

        public static Disaster CreateArchived()
        {
            var disaster = CreateClosedWithReport(out _);
            disaster.Archive();
            return disaster;
        }

        public static Disaster CreateCancelled(DateTimeOffset? cancelledAt = null)
        {
            var disaster = CreateReported();
            disaster.Cancel(cancelledAt ?? new DateTimeOffset(2026, 1, 5, 8, 0, 0, TimeSpan.Zero));
            return disaster;
        }
    }
}

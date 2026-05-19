namespace Eghatha.Domain.Disasters
{
    public static class DisasterTimelineEventTypes
    {
        public const string Created = "Created";
        public const string ResponseStarted = "ResponseStarted";
        public const string TeamAssigned = "TeamAssigned";
        public const string VolunteerAssigned = "VolunteerAssigned";
        public const string ResourceUpdated = "ResourceUpdated";
        public const string AffectedPersonAdded = "AffectedPersonAdded";
        public const string ReportGenerated = "ReportGenerated";
        public const string Closed = "Closed";
        public const string Cancelled = "Cancelled";
        public const string Resolved = "Resolved";

    }
}

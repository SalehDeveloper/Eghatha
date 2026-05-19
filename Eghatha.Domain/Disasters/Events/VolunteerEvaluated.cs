using Eghatha.Domain.Abstractions;

namespace Eghatha.Domain.Disaster
{
    public sealed class VolunteerEvaluated : DomainEvent
    {
        public VolunteerEvaluated(
            Guid volunteerId,
            Guid disasterId,
            int totalScore,
            DateTimeOffset evaluatedAt)
        {
            VolunteerId = volunteerId;
            DisasterId = disasterId;
            TotalScore = totalScore;
            EvaluatedAt = evaluatedAt;
        }

        public Guid VolunteerId { get; }

        public Guid DisasterId { get; }

        public int TotalScore { get; }

        public DateTimeOffset EvaluatedAt { get; }
    }
}
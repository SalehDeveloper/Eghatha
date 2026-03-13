using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Disasters.DisasterVolunteers
{
    public sealed record EvaluationScores
    {
        private EvaluationScores(
            int commitmentScore,
            int skillScore,
            int safetyScore,
            int teamWorkScore,
            int initiativeScore)
        {
            CommitmentScore = commitmentScore;
            this.skillScore = skillScore;
            SafetyScore = safetyScore;
            TeamWorkScore = teamWorkScore;
            InitiativeScore = initiativeScore;
        }

        public int CommitmentScore { get; private set; }

        public int skillScore { get; private set; }

        public int SafetyScore { get; private set; }

        public int TeamWorkScore { get; private set; }

        public int InitiativeScore { get; private set; }


        public static ErrorOr<EvaluationScores> Create (
        int commitmentScore,
        int skillScore,
        int safetyScore,
        int teamWorkScore,
        int initiativeScore)
        {
            if (commitmentScore is < 1 or > 5)
                return DisasterErrors.InvalidScore;

            if (skillScore is < 1 or > 5)
                return DisasterErrors.InvalidScore;

            if (safetyScore is < 1 or > 5)
                return DisasterErrors.InvalidScore;

            if (teamWorkScore is < 1 or > 5)
                return DisasterErrors.InvalidScore;

            if (initiativeScore is < 1 or > 5)
                return DisasterErrors.InvalidScore;

            return new EvaluationScores(commitmentScore , skillScore , safetyScore , teamWorkScore , initiativeScore);
        }



    }
}

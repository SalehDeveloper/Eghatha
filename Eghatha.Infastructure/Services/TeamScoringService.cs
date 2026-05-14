using Eghatha.Application.Common.Models;
using Eghatha.Application.Common.Services;
using Eghatha.Domain.Disasters;
using Eghatha.Domain.Teams;
using Eghatha.Domain.Teams.TeamMembers;
using Microsoft.EntityFrameworkCore;

namespace Eghatha.Infastructure.Services
{
    public sealed class TeamScoringService : ITeamScoringService
    {
        public double Calculate(
            Team team,
            Disaster disaster,
            RouteResult route)
        {
            double operationalScore = 0;

            // ----------------------------------------------------
            // Speciality Match
            // ----------------------------------------------------

            if (disaster.Type.RecommendedTeamSpecialities
                .Contains(team.Speciality))
            {
                operationalScore += 30;
            }

            // ----------------------------------------------------
            // Active Members
            // ----------------------------------------------------

            var activeMembers = team.Members
                .Count(x => x.Status == TeamMemberStatus.Active);

            double memberScore =
                Math.Min(activeMembers * 1.5, 15);

            operationalScore += memberScore;

            // ----------------------------------------------------
            // Resources
            // ----------------------------------------------------

            var totalResources = team.Resources
                .Sum(x => x.Quantity);

            double resourceScore =
                Math.Min(totalResources / 5.0, 10);

            operationalScore += resourceScore;

            // ----------------------------------------------------
            // Team Status
            // ----------------------------------------------------

            if (team.Status == TeamStatus.Active)
                operationalScore += 10;

            if (team.Status == TeamStatus.Returning)
                operationalScore += 5;

            // ====================================================
            // 3. ETA Factor
            // ====================================================

            double etaFactor =
                Math.Exp(-route.DurationMinutes / 45.0);

            // ====================================================
            // 4. Final Score
            // ====================================================

            double finalScore =
                operationalScore * etaFactor;

            return Math.Round(
                Math.Max(finalScore, 0),
                2);
        }
    }
}

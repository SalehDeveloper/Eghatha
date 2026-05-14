using Eghatha.Application.Common.Models;
using Eghatha.Application.Common.Services;
using Eghatha.Domain.Disasters;
using Eghatha.Domain.Volunteers;
using Eghatha.Domain.Volunteers.Equipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Services
{
    public class VolunteerScoringService : IVolunteerScoringService
    {
        public double Calculate(Disaster disaster, Volunteer volunteer, RouteResult route)
        {
            double operationalScore = 0;

            // ====================================================
            // 1. Required Speciality
            // ====================================================

            if (disaster.Type.RequiredVolunteerSpecialities
                .Contains(volunteer.Speciality))
            {
                operationalScore += 30;
            }

            // ====================================================
            // 2. Experience
            // ====================================================

            double experienceScore =
                Math.Min(
                    volunteer.YearsOfExperience * 1.5,
                    15);

            operationalScore += experienceScore;

            // ====================================================
            // 3. Volunteer Rating
            // ====================================================

            double ratingScore =
                Math.Min(
                    volunteer.AverageScore * 5,
                    15);

            operationalScore += ratingScore;

            // ====================================================
            // 4. Equipment Score
            // ====================================================

            var availableEquipments = volunteer.Equipments
                .Count(x =>
                    x.Status == EquipmentStatus.Valid &&
                    x.Quantity > 0);

            double equipmentScore =
                Math.Min(
                    availableEquipments * 2,
                    10);

            operationalScore += equipmentScore;

            // ====================================================
            // 5. Volunteer Status
            // ====================================================

            if (volunteer.Status == VolunteerStatus.Available)
                operationalScore += 10;

            // ====================================================
            // 6. ETA Factor
            // ====================================================

            double etaFactor =
                Math.Exp(-route.DurationMinutes / 45.0);

            // ====================================================
            // 7. Final Score
            // ====================================================

            double finalScore =
                operationalScore * etaFactor;

            return Math.Round(
                Math.Max(finalScore, 0),
                2);
        }
    }
}

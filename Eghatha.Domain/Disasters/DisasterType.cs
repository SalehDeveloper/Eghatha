using Ardalis.SmartEnum;
using Eghatha.Domain.Teams;
using Eghatha.Domain.Volunteers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Disasters
{
    public class DisasterType : SmartEnum<DisasterType>
    {

        public static readonly DisasterType Fire =
          new(
              nameof(Fire),
              1,
              new List<VolunteerSpeciality>
              {
                VolunteerSpeciality.FireFighting,
                VolunteerSpeciality.FirstAid,
                VolunteerSpeciality.Medical,
                VolunteerSpeciality.Logistics
              },
              new List<TeamSpeciality>
              {
                TeamSpeciality.FireFightingTeam,
                TeamSpeciality.MedicalTeam,
                TeamSpeciality.LogisticsTeam,
                TeamSpeciality.RapidResponseTeam
              });

        public static readonly DisasterType Earthquake =
            new(
                nameof(Earthquake),
                2,
                new List<VolunteerSpeciality>
                {
                VolunteerSpeciality.SearchAndRescue,
                VolunteerSpeciality.Medical,
                VolunteerSpeciality.Engineering,
                VolunteerSpeciality.Logistics
                },
                new List<TeamSpeciality>
                {
                TeamSpeciality.SearchAndRescueTeam,
                TeamSpeciality.EngineeringTeam,
                TeamSpeciality.MedicalTeam,
                TeamSpeciality.HeavyRescueTeam,
                TeamSpeciality.LogisticsTeam
                });

        public static readonly DisasterType Explosion =
            new(
                nameof(Explosion),
                3,
                new List<VolunteerSpeciality>
                {
                VolunteerSpeciality.SearchAndRescue,
                VolunteerSpeciality.Medical,
                VolunteerSpeciality.Engineering,
                VolunteerSpeciality.Communications
                },
                new List<TeamSpeciality>
                {
                TeamSpeciality.SearchAndRescueTeam,
                TeamSpeciality.EngineeringTeam,
                TeamSpeciality.MedicalTeam,
                TeamSpeciality.CommunicationsTeam,
                TeamSpeciality.HeavyRescueTeam
                });

        public static readonly DisasterType Landslide =
            new(
                nameof(Landslide),
                4,
                new List<VolunteerSpeciality>
                {
                VolunteerSpeciality.SearchAndRescue,
                VolunteerSpeciality.Engineering,
                VolunteerSpeciality.Medical
                },
                new List<TeamSpeciality>
                {
                TeamSpeciality.SearchAndRescueTeam,
                TeamSpeciality.EngineeringTeam,
                TeamSpeciality.HeavyRescueTeam,
                TeamSpeciality.MedicalTeam
                });

        public static readonly DisasterType ChemicalSpill =
            new(
                nameof(ChemicalSpill),
                5,
                new List<VolunteerSpeciality>
                {
                VolunteerSpeciality.Engineering,
                VolunteerSpeciality.Medical,
                VolunteerSpeciality.Logistics,
                VolunteerSpeciality.Communications
                },
                new List<TeamSpeciality>
                {
                TeamSpeciality.HazardousMaterialsTeam,
                TeamSpeciality.EngineeringTeam,
                TeamSpeciality.MedicalTeam,
                TeamSpeciality.CommunicationsTeam,
                TeamSpeciality.LogisticsTeam
                });

        public static readonly DisasterType MedicalEmergency =
            new(
                nameof(MedicalEmergency),
                6,
                new List<VolunteerSpeciality>
                {
                VolunteerSpeciality.Medical,
                VolunteerSpeciality.FirstAid,
                VolunteerSpeciality.Logistics
                },
                new List<TeamSpeciality>
                {
                TeamSpeciality.MedicalTeam,
                TeamSpeciality.RapidResponseTeam,
                TeamSpeciality.LogisticsTeam
                });

        public static readonly DisasterType StormOrTornado =
            new(
                nameof(StormOrTornado),
                7,
                new List<VolunteerSpeciality>
                {
                VolunteerSpeciality.SearchAndRescue,
                VolunteerSpeciality.Communications,
                VolunteerSpeciality.Medical,
                VolunteerSpeciality.Logistics
                },
                new List<TeamSpeciality>
                {
                TeamSpeciality.SearchAndRescueTeam,
                TeamSpeciality.CommunicationsTeam,
                TeamSpeciality.MedicalTeam,
                TeamSpeciality.LogisticsTeam,
                TeamSpeciality.EvacuationTeam
                });

        public static readonly DisasterType TrafficAccident =
            new(
                nameof(TrafficAccident),
                8,
                new List<VolunteerSpeciality>
                {
                VolunteerSpeciality.FirstAid,
                VolunteerSpeciality.Medical,
                VolunteerSpeciality.Logistics,
                VolunteerSpeciality.HeavyEquipmentOperator
                },
                new List<TeamSpeciality>
                {
                TeamSpeciality.MedicalTeam,
                TeamSpeciality.RapidResponseTeam,
                TeamSpeciality.LogisticsTeam,
                TeamSpeciality.HeavyRescueTeam
                });

        public static readonly DisasterType FireInIndustrialSite =
            new(
                nameof(FireInIndustrialSite),
                9,
                new List<VolunteerSpeciality>
                {
                VolunteerSpeciality.FireFighting,
                VolunteerSpeciality.Medical,
                VolunteerSpeciality.Engineering,
                VolunteerSpeciality.FirstAid
                },
                new List<TeamSpeciality>
                {
                TeamSpeciality.FireFightingTeam,
                TeamSpeciality.EngineeringTeam,
                TeamSpeciality.HazardousMaterialsTeam,
                TeamSpeciality.MedicalTeam
                });

        public static readonly DisasterType WaterRescue =
            new(
                nameof(WaterRescue),
                10,
                new List<VolunteerSpeciality>
                {
                VolunteerSpeciality.WaterRescue,
                VolunteerSpeciality.FirstAid,
                VolunteerSpeciality.Medical
                },
                new List<TeamSpeciality>
                {
                TeamSpeciality.WaterRescueTeam,
                TeamSpeciality.MedicalTeam,
                TeamSpeciality.RapidResponseTeam
                });

        public static readonly DisasterType MountainRescue =
            new(
                nameof(MountainRescue),
                11,
                new List<VolunteerSpeciality>
                {
                VolunteerSpeciality.MountainRescue,
                VolunteerSpeciality.SearchAndRescue,
                VolunteerSpeciality.Medical
                },
                new List<TeamSpeciality>
                {
                TeamSpeciality.MountainRescueTeam,
                TeamSpeciality.SearchAndRescueTeam,
                TeamSpeciality.MedicalTeam
                });

        public static readonly DisasterType Flood =
            new(
                nameof(Flood),
                12,
                new List<VolunteerSpeciality>
                {
                VolunteerSpeciality.WaterRescue,
                VolunteerSpeciality.FirstAid,
                VolunteerSpeciality.Medical
                },
                new List<TeamSpeciality>
                {
                TeamSpeciality.WaterRescueTeam,
                TeamSpeciality.EvacuationTeam,
                TeamSpeciality.LogisticsTeam,
                TeamSpeciality.MedicalTeam
                });

        public static readonly DisasterType Other =
            new(
                nameof(Other),
                99,
                new List<VolunteerSpeciality>
                {
                VolunteerSpeciality.General,
                VolunteerSpeciality.Logistics,
                VolunteerSpeciality.FirstAid
                },
                new List<TeamSpeciality>
                {
                TeamSpeciality.GeneralSupportTeam,
                TeamSpeciality.RapidResponseTeam,
                TeamSpeciality.LogisticsTeam,
                TeamSpeciality.MedicalTeam
                });

        public IReadOnlyList<VolunteerSpeciality> RequiredVolunteerSpecialities { get; }

        public IReadOnlyList<TeamSpeciality> RecommendedTeamSpecialities { get; }

        public DisasterType(
            string name,
            int value,
            List<VolunteerSpeciality> requiredVolunteerSpecialities,
            List<TeamSpeciality> recommendedTeamSpecialities)
            : base(name, value)
        {
            RequiredVolunteerSpecialities = requiredVolunteerSpecialities;

            RecommendedTeamSpecialities = recommendedTeamSpecialities;
        }
    }
}

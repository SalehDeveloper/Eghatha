using Ardalis.SmartEnum;
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

        public static readonly DisasterType Fire = new(nameof(Fire), 1, new List<VolunteerSpeciality>
    {
        VolunteerSpeciality.FireFighting,
        VolunteerSpeciality.FirstAid,
        VolunteerSpeciality.Medical,
        VolunteerSpeciality.Logistics
    });

        public static readonly DisasterType Earthquake = new(nameof(Earthquake), 2, new List<VolunteerSpeciality>
    {
        VolunteerSpeciality.SearchAndRescue,
        VolunteerSpeciality.Medical,
        VolunteerSpeciality.Engineering,
        VolunteerSpeciality.Logistics
    });

        public static readonly DisasterType Explosion = new(nameof(Explosion), 3, new List<VolunteerSpeciality>
    {
        VolunteerSpeciality.SearchAndRescue,
        VolunteerSpeciality.Medical,
        VolunteerSpeciality.Engineering,
        VolunteerSpeciality.Communications
    });

        public static readonly DisasterType Landslide = new(nameof(Landslide), 4, new List<VolunteerSpeciality>
    {
        VolunteerSpeciality.SearchAndRescue,
        VolunteerSpeciality.Engineering,
        VolunteerSpeciality.Medical
    });

        public static readonly DisasterType ChemicalSpill = new(nameof(ChemicalSpill), 5, new List<VolunteerSpeciality>
    {
        VolunteerSpeciality.Engineering,
        VolunteerSpeciality.Medical,
        VolunteerSpeciality.Logistics,
        VolunteerSpeciality.Communications
    });

        public static readonly DisasterType MedicalEmergency = new(nameof(MedicalEmergency), 6, new List<VolunteerSpeciality>
    {
        VolunteerSpeciality.Medical,
        VolunteerSpeciality.FirstAid,
        VolunteerSpeciality.Logistics
    });

        public static readonly DisasterType StormOrTornado = new(nameof(StormOrTornado), 7, new List<VolunteerSpeciality>
    {
        VolunteerSpeciality.SearchAndRescue,
        VolunteerSpeciality.Communications,
        VolunteerSpeciality.Medical,
        VolunteerSpeciality.Logistics
    });

        public static readonly DisasterType TrafficAccident = new(nameof(TrafficAccident), 8, new List<VolunteerSpeciality>
    {
        VolunteerSpeciality.FirstAid,
        VolunteerSpeciality.Medical,
        VolunteerSpeciality.Logistics,
        VolunteerSpeciality.HeavyEquipmentOperator
    });

        public static readonly DisasterType FireInIndustrialSite = new(nameof(FireInIndustrialSite), 9, new List<VolunteerSpeciality>
    {
        VolunteerSpeciality.FireFighting,
        VolunteerSpeciality.Medical,
        VolunteerSpeciality.Engineering,
        VolunteerSpeciality.FirstAid
    });

        public static readonly DisasterType WaterRescue = new(nameof(WaterRescue), 10, new List<VolunteerSpeciality>
    {
        VolunteerSpeciality.WaterRescue,
        VolunteerSpeciality.FirstAid,
        VolunteerSpeciality.Medical
    });

        public static readonly DisasterType MountainRescue = new(nameof(MountainRescue), 11, new List<VolunteerSpeciality>
    {
        VolunteerSpeciality.MountainRescue,
        VolunteerSpeciality.SearchAndRescue,
        VolunteerSpeciality.Medical
    });

        public static readonly DisasterType Flood = new(nameof(Flood), 12, new List<VolunteerSpeciality>
    {
        VolunteerSpeciality.WaterRescue,
        VolunteerSpeciality.FirstAid,
        VolunteerSpeciality.Medical
    });

        public static readonly DisasterType Other = new(nameof(Other), 99, new List<VolunteerSpeciality>
        {
            VolunteerSpeciality.General , 
            VolunteerSpeciality.Logistics,
            VolunteerSpeciality.FirstAid
        });
          


        public IReadOnlyList<VolunteerSpeciality> RequiredSpecialities { get; }
        
        public DisasterType(string name, int value , List<VolunteerSpeciality> requiredSpecialities) 
            : base(name, value)
        {
            RequiredSpecialities = requiredSpecialities;
        }

    }
}

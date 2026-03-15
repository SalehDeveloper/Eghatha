using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Teams.Resources
{
    public class ResourceType : SmartEnum<ResourceType>
    {
        public bool IsConsumable { get; }

        // Fire Fighting
        public static readonly ResourceType FireTruck = new(nameof(FireTruck), 1, false);
        public static readonly ResourceType FireExtinguisher = new(nameof(FireExtinguisher), 2, true);
        public static readonly ResourceType FireHose = new(nameof(FireHose), 3, false);

        // Medical
        public static readonly ResourceType Ambulance = new(nameof(Ambulance), 4, false);
        public static readonly ResourceType FirstAidKit = new(nameof(FirstAidKit), 5, true);
        public static readonly ResourceType MedicalSupplies = new(nameof(MedicalSupplies), 6, true);
        public static readonly ResourceType Stretcher = new(nameof(Stretcher), 7, false);

        // Search & Rescue
        public static readonly ResourceType RescueTools = new(nameof(RescueTools), 8, false);
        public static readonly ResourceType ThermalCamera = new(nameof(ThermalCamera), 9, false);
        public static readonly ResourceType RescueDog = new(nameof(RescueDog), 10, false);

        // Engineering
        public static readonly ResourceType Excavator = new(nameof(Excavator), 11, false);
        public static readonly ResourceType Bulldozer = new(nameof(Bulldozer), 12, false);
        public static readonly ResourceType Crane = new(nameof(Crane), 13, false);

        // Logistics
        public static readonly ResourceType TransportTruck = new(nameof(TransportTruck), 14, false);
        public static readonly ResourceType Tent = new(nameof(Tent), 15, false);
        public static readonly ResourceType FoodSupplies = new(nameof(FoodSupplies), 16, true);
        public static readonly ResourceType WaterSupplies = new(nameof(WaterSupplies), 17, true);
        public static readonly ResourceType Generator = new(nameof(Generator), 18, false);

        // Communications
        public static readonly ResourceType RadioDevice = new(nameof(RadioDevice), 19, false);
        public static readonly ResourceType SatellitePhone = new(nameof(SatellitePhone), 20, false);
        public static readonly ResourceType MobileCommandCenter = new(nameof(MobileCommandCenter), 21, false);

        // Water Rescue
        public static readonly ResourceType RescueBoat = new(nameof(RescueBoat), 22, false);
        public static readonly ResourceType LifeJacket = new(nameof(LifeJacket), 23, false);

        // Mountain Rescue
        public static readonly ResourceType ClimbingGear = new(nameof(ClimbingGear), 24, false);
        public static readonly ResourceType RopeKit = new(nameof(RopeKit), 25, false);

        public ResourceType(string name, int value, bool isConsumable) : base(name, value)
        {
            IsConsumable = isConsumable;
        }
    }
}

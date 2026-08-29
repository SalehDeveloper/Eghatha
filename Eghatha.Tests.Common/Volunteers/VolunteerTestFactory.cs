using Eghatha.Domain.Volunteers;
using Eghatha.Domain.Volunteers.Equipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Tests.Common.Volunteers
{
    /// <summary>
    /// Produces Volunteer aggregates in commonly-needed shapes for tests.
    /// Every mutation goes through the aggregate's own public methods
    /// (never reflection/internal state hacks), so these helpers stay
    /// valid as long as Volunteer's own public API doesn't change.
    /// </summary>
    public static class VolunteerTestFactory
    {
        public static Volunteer CreateValid() => VolunteerBuilder.Valid().BuildValid();

        /// <summary>
        /// A valid volunteer with a single Medical-category equipment item
        /// already attached, for tests exercising Update/Remove/Increase/
        /// Decrease equipment paths without repeating the AddEquipment
        /// arrange step in every test.
        /// </summary>
        public static Volunteer CreateWithEquipment(out Equipment equipment, int quantity = 5)
        {
            var volunteer = CreateValid();
            equipment = volunteer.AddEquipment("First Aid Kit", EquipmentCategory.Medical, quantity).Value;
            return volunteer;
        }
    }
}

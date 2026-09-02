using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Volunteers.Equipments
{
    public static class EquipmentErrors
    {
        public static readonly Error IdRequired = Error.Validation(
            code: "VolunteerErrors.Equipment.IdRequired",
            description: "VolunteerErrors.equipment id is required");

        public static readonly Error NameRequired = Error.Validation(
          code: "VolunteerErrors.Equipment.NameRequired",
          description: "equipment name is required");


        public static readonly Error UnSupportedCategory = Error.Validation(
          code: "VolunteerErrors.Equipment.UnSupportedCategory",
          description: "equipment category unsupported");

        public static readonly Error InvalidStatus = Error.Validation(
         code: "VolunteerErrors.Equipment.InvalidStatus",
         description: "invalid requpment status ");


        public static readonly Error QuantityShouldBeGreaterThanZero = Error.Validation(
          code: "VolunteerErrors.Equipment.QuantityShouldBeGreaterThanZero",
          description: "equipment quantity should be greater than zero");

        public static readonly Error AlreadyDeleted = Error.Conflict(
            code: "VolunteerErrors.Equipment.AlreadyDeleted",
            description: "equipemtn already deleted");

        public static readonly Error NotFound = Error.NotFound(
            code: "VolunteerErrors.Equipment.NotFound",
            description: "equipment not found");

        public static readonly Error NotEnoughEquipments = Error.Conflict(
        code: "VolunteerErrors.Equipment.NotEnough",
        description: "Not enough equipment available to perform the operation."


    );


    }
}

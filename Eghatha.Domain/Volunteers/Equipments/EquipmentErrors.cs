using ErrorOr;

namespace Eghatha.Domain.Volunteers.Equipments
{
    public static class EquipmentErrors
    {
        public static readonly Error IdRequired = Error.Validation(
            code: "VolunteerErrors.Equipment.IdRequired",
            description: Resources.VolunteerErrors.VolunteerErrors_EquipmentRequired);

        public static readonly Error NameRequired = Error.Validation(
          code: "VolunteerErrors.Equipment.NameRequired",
          description: Resources.VolunteerErrors.VolunteerErrors_Equipment_NameRequired);


        public static readonly Error UnSupportedCategory = Error.Validation(
          code: "VolunteerErrors.Equipment.UnSupportedCategory",
          description: Resources.VolunteerErrors.VolunteerErrors_Equipment_UnSupportedCategory);

        public static readonly Error InvalidStatus = Error.Validation(
         code: "VolunteerErrors.Equipment.InvalidStatus",
         description: Resources.VolunteerErrors.VolunteerErrors_Equipment_InvalidStatus);


        public static readonly Error QuantityShouldBeGreaterThanZero = Error.Validation(
          code: "VolunteerErrors.Equipment.QuantityShouldBeGreaterThanZero",
          description: Resources.VolunteerErrors.VolunteerErrors_Equipment_QuantityShouldBeGreaterThanZero);

        public static readonly Error AlreadyDeleted = Error.Conflict(
            code: "VolunteerErrors.Equipment.AlreadyDeleted",
            description: Resources.VolunteerErrors.VolunteerErrors_Equipment_AlreadyDeleted);

        public static readonly Error NotFound = Error.NotFound(
            code: "VolunteerErrors.Equipment.NotFound",
            description: Resources.VolunteerErrors.VolunteerErrors_Equipment_NotFound);

        public static readonly Error NotEnoughEquipments = Error.Conflict(
        code: "VolunteerErrors.Equipment.NotEnough",
        description: Resources.VolunteerErrors.VolunteerErrors_Equipment_NotEnough);





    }
}

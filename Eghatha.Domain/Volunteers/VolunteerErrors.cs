using ErrorOr;

namespace Eghatha.Domain.Volunteers
{
    public static class VolunteerErrors
    {
        public static Error StatusRequired = Error.Validation(
            code: "VolunteerErrors.StatusRequired",
            description: Resources.VolunteerErrors.VolunteerErrors_StatusRequired);

        public static Error StatusInvalid = Error.Validation(
            code: "VolunteerErrors.StatusInvalid",
            description: Resources.VolunteerErrors.VolunteerErrors_StatusInvalid);

        public static Error SpecialityRequired = Error.Validation(
            code: "VolunteerErrors.SpecialityRequired",
            description: Resources.VolunteerErrors.VolunteerErrors_SpecialityRequired);

        public static Error SpecialityInvalid = Error.Validation(
            code: "VolunteerErrors.SpecialityInvalid",
            description: Resources.VolunteerErrors.VolunteerErrors_SpecialityInvalid);

        public static Error LocationRequired = Error.Validation(
          code: "VolunteerErrors.LocationRequired",
          description: Resources.VolunteerErrors.VolunteerErrors_LocationRequired);

        public static Error ExperienceMustBeGreaterThanZero = Error.Validation(
          code: "VolunteerErrors.ExperienceMustBeGreaterThanZero",
          description: Resources.VolunteerErrors.VolunteerErrors_ExperienceMustBeGreaterThanZero);


        public static Error EquipmentRequired = Error.Validation(
            code: "VolunteerErrors.EquipmentRequired",
            description: Resources.VolunteerErrors.VolunteerErrors_EquipmentRequired);

        public static Error EquipmentAlreadyAssigned = Error.Conflict(
            code: "VolunteerErrors.EquipmentAlreadyAssigned",
            description: Resources.VolunteerErrors.VolunteerErrors_EquipmentAlreadyAssigned);

        public static Error ScoreMustBeGreaterThanZero = Error.Validation(
            code: "VolunteerErrors.ScoreMustBeGreaterThanZero",
            description: Resources.VolunteerErrors.VolunteerErrors_ScoreMustBeGreaterThanZero);

        public static Error CityRequired = Error.Validation(
          code: "VolunteerErrors.City.Required",
          description: Resources.VolunteerErrors.VolunteerErrors_City_Required);

        public static Error ProvinceRequired = Error.Validation(
            code: "VolunteerErrors.Province.Required",
            description: Resources.VolunteerErrors.VolunteerErrors_Province_Required);

    }
}

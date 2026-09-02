using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Volunteers
{
    public static class VolunteerErrors
    {
        public static Error StatusRequired = Error.Validation(
            code: "VolunteerErrors.StatusRequired",
            description: "volunteer status is required");

        public static Error StatusInvalid = Error.Validation(
            code: "VolunteerErrors.StatusInvalid",
            description: "invalid volunteer status ");

        public static Error SpecialityRequired = Error.Validation(
            code: "VolunteerErrors.SpecialityRequired",
            description: "volunteer status is required");

        public static Error SpecialityInvalid = Error.Validation(
            code: "VolunteerErrors.SpecialityInvalid",
            description: "invalid volunteer status ");

        public static Error LocationRequired = Error.Validation(
          code: "VolunteerErrors.LocationRequired",
          description: "volunteer location is required");

        public static Error ExperienceMustBeGreaterThanZero = Error.Validation(
          code: "VolunteerErrors.ExperienceMustBeGreaterThanZero",
          description: "years of experience must be greater than zero");


        public static Error EquipmentRequired = Error.Validation(
            code: "VolunteerErrors.EquipmentRequired",
            description: "volunteer equipment is required");

        public static Error EquipmentAlreadyAssigned = Error.Conflict(
            code: "VolunteerErrors.EquipmentAlreadyAssigned",
            description: "volnteer equipments already has this equipment");

        public static Error ScoreMustBeGreaterThanZero = Error.Validation(
            code: "VolunteerErrors.ScoreMustBeGreaterThanZero",
            description: "Volunteer score should be greater than zero");

        public static Error CityRequired = Error.Validation(
          code: "VolunteerErrors.City.Required",
          description: "Team city is required.");

        public static Error ProvinceRequired = Error.Validation(
            code: "VolunteerErrors.Province.Required",
            description: "Volunteer province is required.");

    }
}

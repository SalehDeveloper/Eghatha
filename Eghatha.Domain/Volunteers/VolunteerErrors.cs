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
            code: "Volunteer.StatusRequired",
            description: "volunteer status is required");

        public static Error StatusInvalid = Error.Validation(
            code: "Volunteer.StatusInvalid",
            description: "invalid volunteer status ");

        public static Error SpecialityRequired = Error.Validation(
            code: "Volunteer.SpecialityRequired",
            description: "volunteer status is required");

        public static Error SpecialityInvalid = Error.Validation(
            code: "Volunteer.SpecialityInvalid",
            description: "invalid volunteer status ");

        public static Error LocationRequired = Error.Validation(
          code: "Volunteer.LocationRequired",
          description: "volunteer location is required");

        public static Error ExperienceMustBeGreaterThanZero = Error.Validation(
          code: "Volunteer.ExperienceMustBeGreaterThanZero",
          description: "years of experience must be greater than zero");


        public static Error EquipmentRequired = Error.Validation(
            code: "Volunteer.EquipmentRequired",
            description: "volunteer equipment is required");

        public static Error EquipmentAlreadyAssigned = Error.Conflict(
            code: "Volunteer.EquipmentAlreadyAssigned",
            description: "volnteer equipments already has this equipment");

        public static Error ScoreMustBeGreaterThanZero = Error.Validation(
            code: "Volunteer.ScoreMustBeGreaterThanZero",
            description: "Volunteer score should be greater than zero");

        public static Error CityRequired = Error.Validation(
          code: "Volunteer.City.Required",
          description: "Team city is required.");

        public static Error ProvinceRequired = Error.Validation(
            code: "Volunteer.Province.Required",
            description: "Volunteer province is required.");

    }
}

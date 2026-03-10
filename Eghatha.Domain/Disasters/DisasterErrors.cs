using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Disasters
{
    public static class DisasterErrors
    {
        public static Error ReporterNameRequired = Error.Validation(
            code: "Disaster.ReporterNameRequired",
            description: "reporter name is required");

        public static Error ReporterContactRequired = Error.Validation(
            code: "Disaster.ReporterContactRequired",
            description: "reporter contact is required");

        public static Error ReporterIdRequired = Error.Validation(
              code: "Disaster.ReporterIdRequired",
            description: "reporter Id is required");

        public static Error TitleRequired = Error.Validation(
           code: "Disaster.TitleRequired",
            description: "title is required");

        public static Error DescriptionRequired = Error.Validation(
        code: "Disaster.DescriptionRequired",
         description: "description is required");

        public static Error LocationRequired = Error.Validation(
        code: "Disaster.LocationRequired",
         description: "location is required");

        public static Error ReporterInfoRequired = Error.Validation(
       code: "Disaster.ReporterInfoRequired",
        description: "reporter info is required");

        



    }
}

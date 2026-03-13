using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Disasters.AffectedPersons
{
    public static class AffectedPersonErrors
    {
        public static readonly Error NameRequired = Error.Validation(
            code: "AffectedPerson.Name.Required",
            description: "Name is required.");

        public static readonly Error InvalidAge = Error.Validation(
           code: "AffectedPerson.Age.Invalid",
          description: "age should be greater than zero.");


    }
}

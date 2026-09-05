using ErrorOr;

namespace Eghatha.Domain.Disasters.AffectedPersons
{
    public static class AffectedPersonErrors
    {
        public static readonly Error NameRequired = Error.Validation(
            code: "AffectedPerson.Name.Required",
            description: Resources.DisasterErrors.DisasterErrors_AffectedPerson_Name_Required);

        public static readonly Error InvalidAge = Error.Validation(
           code: "AffectedPerson.Age.Invalid",
          description: Resources.DisasterErrors.DisasterErrors_AffectedPerson_Age_Invalid);

        public static readonly Error InvalidStatus = Error.Validation(
         code: "AffectedPerson.Status.Invalid",
        description: Resources.DisasterErrors.DisasterErrors_AffectedPerson_Status_Invalid);


    }
}
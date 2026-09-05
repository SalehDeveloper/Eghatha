using ErrorOr;

namespace Eghatha.Domain.Shared.Errors
{
    public static class DomainErrors
    {
        public static readonly Error UserIdRequired = Error.Validation(
            code: "User.UserId.Required",
            description: Resources.DomainErrors.User_UserId_Required);


        public static Error IdMustBeProvided(string entityName) => Error.Validation(
            code: "Entity.IdMustBeProvided",
            description: $"{entityName} Id cannot be empty.");


    }
}
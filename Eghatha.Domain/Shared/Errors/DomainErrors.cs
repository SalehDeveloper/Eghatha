using ErrorOr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Shared.Errors
{
    public static class DomainErrors
    {
        public static readonly Error UserIdRequired = Error.Validation(
            code: "User.UserId.Required",
            description: "UserId is required .");


        public static Error IdMustBeProvided(string entityName) => Error.Validation(
            code: "Entity.IdMustBeProvided",
            description: $"{entityName} Id cannot be empty.");


    }
}

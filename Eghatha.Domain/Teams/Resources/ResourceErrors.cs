using Ardalis.SmartEnum;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Teams.Resources
{
    public static class ResourceErrors
    {
        public static readonly Error InvalidResourceType = Error.Validation(
            code: "Resource.InvalidType",
            description: "The provided resource type is invalid."
        );

        public static readonly Error ResourceTypeRequired = Error.Validation(
            code: "Resource.TypeRequired",
            description: "The resource type is required."
        );

        public static readonly Error StatusRequired = Error.Validation(
            code: "Resource.StatusRequired",
            description: "The resource status is required."
        );


        public static readonly Error QuantityShouldBeGreaterThanZero = Error.Validation(
            code: "Resource.QuantityInvalid",
            description: "The quantity should be greater than or equal to zero.");

        public static readonly Error   NotEnoughResources = Error.Conflict(
            code: "Resource.NotEnough",
            description: "Not enough resources available to perform the operation."


        );
    
        public static Error NotFound = Error.NotFound(
            code: "Resource.NotFound",
            description: "The specified resource was not found."
        );
    }
}

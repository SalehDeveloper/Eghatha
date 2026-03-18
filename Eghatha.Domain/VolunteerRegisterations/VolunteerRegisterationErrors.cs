using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.VolunteerRegisterations
{
    public static class VolunteerRegisterationErrors
    {
        public static readonly Error AlreadyProcessed = Error.Conflict(
            code: "VolunteerRegisterationErrors.AlreadyProcessed",
            description: "registeration rqeuest already processed");
    }
}

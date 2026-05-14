using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Disasters.Requests
{
    public sealed record CreateDisasterRequest(string Title,
     string Description,
     double Latitude,
     double Longitude,
     string DisasterType,
     string? CustomTypeDescription,
     string ReporterName,
     string ReporterPhone,
     string ReporterNationalId);


}

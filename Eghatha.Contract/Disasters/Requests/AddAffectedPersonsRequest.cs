using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Disasters.Requests
{
    public sealed record AddAffectedPersonsRequest(
     List<AffectedPersonDto> Persons);

    public sealed record AffectedPersonDto( string Name,int Age,string Phone,string Status,string? Notes);

}

using Eghatha.Domain.Teams.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Teams
{
    public sealed record AddResourceResult(
    Resource Resource,
    bool IsNew);
}

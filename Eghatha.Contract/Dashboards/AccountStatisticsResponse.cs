using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Dashboards
{
    public record AccountStatisticsResponse(int TotalUsers, int ActiveUsers, int InActiveUsers, int TotalAdmins);
    
}

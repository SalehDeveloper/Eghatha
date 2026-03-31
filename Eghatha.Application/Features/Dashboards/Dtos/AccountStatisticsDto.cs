using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Dashboards.Dtos
{
    public record AccountStatisticsDto(int TotalUsers, int ActiveUsers, int InActiveUsers, int TotalAdmins);
    
    
}

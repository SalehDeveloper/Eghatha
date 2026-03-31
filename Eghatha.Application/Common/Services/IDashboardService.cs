using Eghatha.Application.Features.Dashboards.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Services
{
    public interface IDashboardService
    {
        public Task<AccountStatisticsDto> GetAccountStatisticsAsync(CancellationToken cancellationToken);
    }
}

using Eghatha.Application.Features.Dashboards.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Dashboards.Queries.AccountStatistics
{
    public record GetAccountStatisticsQuery : ICachedQuery<AccountStatisticsDto>
    {
        public string CachKey => "dashboard:accounts:statistics";

        public string[] Tags => ["dashboard","accounts"];


        public TimeSpan Expiration => TimeSpan.FromMinutes(5);
    }
}

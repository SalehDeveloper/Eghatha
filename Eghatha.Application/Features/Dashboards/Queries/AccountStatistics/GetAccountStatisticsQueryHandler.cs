using Eghatha.Application.Common.Services;
using Eghatha.Application.Features.Dashboards.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Dashboards.Queries.AccountStatistics
{
    internal class GetAccountStatisticsQueryHandler : IRequestHandler<GetAccountStatisticsQuery, AccountStatisticsDto>
    { 
        private readonly IDashboardService _service;

        public GetAccountStatisticsQueryHandler(IDashboardService service)
        {
            _service = service;
        }

        public async Task<AccountStatisticsDto> Handle(GetAccountStatisticsQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetAccountStatisticsAsync(cancellationToken);
        }
    }
}

using Eghatha.Application.Features.Dashboards.Queries.AccountStatistics;
using Eghatha.Contract.Accounts.Responses;
using Eghatha.Contract.Dashboards;
using Eghatha.Contract.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Eghatha.Api.Controllers
{
    public class DashboardsController : ApiController
    {
        public DashboardsController(ISender sender) : base(sender)
        {
        }



        [HttpGet(ApiEndpoints.Dashboards.AccountStatistics)]
        [ProducesResponseType(typeof(AccountStatisticsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAccountStatistics(CancellationToken ct)
        {
            var result = await _sender.Send(new GetAccountStatisticsQuery(), ct);

            return Ok(result);
        }
    }
}

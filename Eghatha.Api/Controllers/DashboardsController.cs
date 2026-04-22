using Eghatha.Application.Features.Dashboards.Queries.AccountStatistics;
using Eghatha.Contract.Accounts.Responses;
using Eghatha.Contract.Dashboards;
using Eghatha.Contract.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eghatha.Api.Controllers
{
    public class DashboardsController : ApiController
    {
        public DashboardsController(ISender sender) : base(sender)
        {
        }


        //[Authorize(Roles =ApplicationRole.Admin)]
        [HttpGet(ApiEndpoints.Dashboards.AccountStatistics)]
        [ProducesResponseType(typeof(AccountStatisticsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Retrieves account statistics.")]
        [EndpointDescription("Returns aggregated statistics about accounts, such as totals  for dashboard display. Requires appropriate authorization.")]
        [EndpointName("GetAccountStatistics")]
        public async Task<IActionResult> GetAccountStatistics(CancellationToken ct)
        {
            var result = await _sender.Send(new GetAccountStatisticsQuery(), ct);

            return Ok(result);
        }
    }
}

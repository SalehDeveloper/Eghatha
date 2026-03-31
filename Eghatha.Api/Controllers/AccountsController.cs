using Eghatha.Api.Mappers;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Accounts.Commands.ActivateAccount;
using Eghatha.Application.Features.Accounts.Commands.DeActivateAccount;
using Eghatha.Application.Features.Accounts.Queries.GetAccounts;
using Eghatha.Contract.Accounts.Requests;
using Eghatha.Contract.Accounts.Responses;
using Eghatha.Contract.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eghatha.Api.Controllers
{
    public class AccountsController : ApiController
    {
        public AccountsController(ISender sender) : base(sender)
        {

        }


        //[Authorize(Roles = ApplicationRole.Admin)]
        [HttpPost(ApiEndpoints.Accounts.Activate)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [EndpointSummary("Activates a user account.")]
        [EndpointDescription("Sets the account status to active for the specified user, Requires Admin privileges.")]
        [EndpointName("ActivateAccount")]
        public async Task<IActionResult> Activate([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var command = new ActivateAccountCommand(id);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                v => base.NoContent(),
                Problem);

        }


        //[Authorize(Roles = ApplicationRole.Admin)]
        [HttpPost(ApiEndpoints.Accounts.DeActivate)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [EndpointSummary("Deactivates a user account.")]
        [EndpointDescription("Sets the account status to inactive for the specified user, Requires Admin privileges.")]
        [EndpointName("DeactivateAccount")]
        public async Task<IActionResult> DeActivate([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var command = new DeActivateAccountCommand(id);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                v => base.NoContent(),
                Problem);

        }


        //[Authorize(Roles = ApplicationRole.Admin)]
        [HttpGet(ApiEndpoints.Accounts.GetAll)]
        [ProducesResponseType(typeof(PagedResponse<AccountResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [EndpointSummary("Retrieve accounts")]
        [EndpointDescription("Returns a paginated list of accounts with optional filtering by role (Admin, TeamMember, Volunteer), active status, and search term.")]
        [EndpointName("GetAccounts")]
        public async Task<IActionResult> GetAccounts([FromQuery] GetAccountsFilters filters, [FromQuery] PagedRequest pageRequest, CancellationToken ct)
        {
            var query = new GetAccountsQuery(pageRequest.Page, pageRequest.PageSize, filters.SearchTerm, filters.Role, filters.IsActive);

            var res = await _sender.Send(query, ct);

            


            return 
                   Ok( new PagedResponse<AccountResponse>(res.PageNumber, res.PageSize, res.TotalPages, res.TotalCount, res.Items.MapToAccounts()));
                    

        }
    }
}

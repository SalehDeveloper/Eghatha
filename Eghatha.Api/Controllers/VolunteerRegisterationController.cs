using Eghatha.Api.Mappers;
using Eghatha.Application.Features.Teams.Queries.GetTeams;
using Eghatha.Application.Features.VolunteerRegisterations.Commands.ApproveRegisteration;
using Eghatha.Application.Features.VolunteerRegisterations.Commands.RejectRegisteration;
using Eghatha.Application.Features.VolunteerRegisterations.Queries.GetAll;
using Eghatha.Contract.Shared;
using Eghatha.Contract.Teams.Requests;
using Eghatha.Contract.Teams.Responses;
using Eghatha.Contract.VolunteerRegisterations.Requests;
using Eghatha.Contract.VolunteerRegisterations.Responses;
using Eghatha.Domain.Teams;
using Eghatha.Domain.VolunteerRegisterations;
using Eghatha.Domain.Volunteers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Eghatha.Api.Controllers
{
    public class VolunteerRegisterationController : ApiController
    {
        public VolunteerRegisterationController(ISender sender) : base(sender)
        {
        }

        // [Authorize(Roles = ApplicationRole.Admin)]
        [HttpPost(ApiEndpoints.VolunteerRegisterations.Approve)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("approve volunteer registeration.")]
        [EndpointDescription("approve volunteer registeration, required admin privilages")]
        [EndpointName("ApproveRegisteration")]
        public async Task<IActionResult> ApproveRegisteration ( Guid registerationid , CancellationToken cancellationToken )
        {
            var command = new ApproveRegisterationCommand(registerationid);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                v=> base.NoContent(),
                Problem);
        }


        // [Authorize(Roles = ApplicationRole.Admin)]
        [HttpPost(ApiEndpoints.VolunteerRegisterations.Reject)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("reject volunteer registeration.")]
        [EndpointDescription("reject volunteer registeration, required admin privilages")]
        [EndpointName("RejectRegisteration")]
        public async Task<IActionResult> RejectRegisteration([FromRoute]Guid registerationid, [FromBody] RejectVolunteerRegiserationRequest request ,  CancellationToken cancellationToken)
        {
            var command = new RejectRegisterationCommand(registerationid , request.Reason);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                v => base.NoContent(),
                Problem);
        }



        // [Authorize(Roles = ApplicationRole.Admin)]
        [HttpGet(ApiEndpoints.VolunteerRegisterations.GetAll)]
        [ProducesResponseType(typeof(PagedResponse<VolunteerRegisterationResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Retrieves volunteer-registerations.")]
        [EndpointDescription("Returns a paginated list of volunteer-registerations with optional filtering by status, search term.")]
        [EndpointName("GetRegisteration")]
        public async Task<IActionResult> GetRegisteration([FromQuery] GetVolunteerRegisteartionsFilter filter, [FromQuery] PagedRequest pagedRequest, CancellationToken cancellationToken)
        {
            RegisterationStatus? status = null;

            if (filter.Status != null)
            {

                if (!RegisterationStatus.TryFromName(filter.Status, true, out var parsed))
                    return Problem(TeamErrors.InvalidSpeciality);
                status = parsed;
            }

           


            var query = new GetAllRegisterationsQuery( pagedRequest.Page , pagedRequest.PageSize ,filter.SearchTerm , status);

            var res = await _sender.Send(query, cancellationToken);

            return
                 Ok(new PagedResponse<VolunteerRegisterationResponse>(res.PageNumber, res.PageSize, res.TotalPages, res.TotalCount, res.Items.ToResponses()));
        }




    }
}

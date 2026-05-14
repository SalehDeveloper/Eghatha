using Eghatha.Api.Mappers;
using Eghatha.Application.Features.Disasters.Commands.AssignTeams;
using Eghatha.Application.Features.Disasters.Commands.AssignVolunteers;
using Eghatha.Application.Features.Disasters.Commands.CreateDisaster;
using Eghatha.Contract.Disasters.Requests;
using Eghatha.Contract.Disasters.Responses;
using Eghatha.Domain.Disasters;
using Eghatha.Domain.Teams;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Eghatha.Api.Controllers
{
    public class DisasterController : ApiController
    {
        public DisasterController(ISender sender) : base(sender)
        {
        }

        [HttpPost(ApiEndpoints.Disasters.Create)]
        [ProducesResponseType(typeof(CreateDisasterResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [EndpointSummary("Creates a new disaster.")]
        [EndpointDescription("Creates a new disaster with the specified details.")]
        [EndpointName("CreateDisaster")]
        public async Task<IActionResult> CreateDisaster ([FromBody] CreateDisasterRequest request , CancellationToken cancellationToken )
        {
            if (!DisasterType.TryFromName(request.DisasterType, true, out var type))
                return Problem(DisasterErrors.InvalidType);

            var command = new CreateDisasterCommand(
                request.Title,
                request.Description,
                request.Latitude,
                request.Longitude,
                type,
                request.CustomTypeDescription,
                request.ReporterName,
                request.ReporterPhone,
                request.ReporterNationalId);

            var res = await _sender.Send(command, cancellationToken);

            return res.Match(
                v => base.Ok(v.ToResponse()),
                Problem);
                
        }


        //[Authorize(ApplicationRole.Admin)]
        [HttpPost(ApiEndpoints.Disasters.AssignTeams)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [EndpointSummary("Assigns teams to a disaster.")]
        [EndpointDescription("Assigns teams to a disaster with the specified details.")]
        [EndpointName("AssignTeams")]
        public async Task<IActionResult> AssignTeams([FromRoute] Guid disasterId, [FromBody] AssignTeamsRequest request, CancellationToken cancellationToken)
        {
            var command = new AssignTeamsCommand(disasterId, request.TeamIds);
            
            var res = await _sender.Send(command, cancellationToken);

            return res.Match(
                v => base.NoContent(),
                Problem);
        }


        //[Authorize(ApplicationRole.Admin)]
        [HttpPost(ApiEndpoints.Disasters.AssignVolunteers)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [EndpointSummary("Assigns volunteers to a disaster.")]
        [EndpointDescription("Assigns volunteers to a disaster with the specified details.")]
        [EndpointName("AssignVolunteers")]
        public async Task<IActionResult> AssignVolunteers([FromRoute] Guid disasterId, [FromBody] AssignVolunteersRequest request, CancellationToken cancellationToken)
        {
            var command = new AssignVolunteersCommand(disasterId, request.VolunteerIds);

            var res = await _sender.Send(command, cancellationToken);

            return res.Match(
                v => base.NoContent(),
                Problem);
        }
    }
}

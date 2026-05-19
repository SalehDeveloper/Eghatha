using Eghatha.Api.Mappers;
using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Disasters.Commands.AddAffectedPersons;
using Eghatha.Application.Features.Disasters.Commands.AssignResource;
using Eghatha.Application.Features.Disasters.Commands.AssignTeams;
using Eghatha.Application.Features.Disasters.Commands.AssignVolunteers;
using Eghatha.Application.Features.Disasters.Commands.CloseDisaster;
using Eghatha.Application.Features.Disasters.Commands.ConsumeResource;
using Eghatha.Application.Features.Disasters.Commands.CreateDisaster;
using Eghatha.Application.Features.Disasters.Commands.EvaluateVolunteer;
using Eghatha.Application.Features.Disasters.Commands.GenerateReport;
using Eghatha.Application.Features.Disasters.Commands.MarkDisasterResourceDamaged;
using Eghatha.Application.Features.Disasters.Commands.ResolveDisaster;
using Eghatha.Application.Features.Disasters.Commands.ReturnResource;
using Eghatha.Application.Features.Disasters.Commands.UpdateAffectedPerson;
using Eghatha.Application.Features.Disasters.Dtos;
using Eghatha.Application.Features.Disasters.Queries.GetAll;
using Eghatha.Application.Features.Disasters.Queries.GetById;
using Eghatha.Application.Features.Disasters.Queries.GetTimeLine;
using Eghatha.Application.Features.Teams.Queries.GetTeams;
using Eghatha.Contract.Disasters.Requests;
using Eghatha.Contract.Disasters.Responses;
using Eghatha.Contract.Shared;
using Eghatha.Contract.Teams.Requests;
using Eghatha.Contract.Teams.Responses;
using Eghatha.Domain.Disasters;
using Eghatha.Domain.Disasters.AffectedPersons;
using Eghatha.Domain.Teams;
using Eghatha.Domain.Teams.Resources;
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


        [HttpPost(ApiEndpoints.Disasters.DispatchResource)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [EndpointSummary("Dispatch resource to a disaster.")]
        [EndpointDescription("Assigns resource to a disaster with the specified details.")]
        [EndpointName("DispatchResource")]
        public async Task<IActionResult> DispatchResource([FromRoute] Guid disasterId,[FromBody] DispatchResourceToDisasterRequest request,  CancellationToken cancellationToken)
        {
           

            var command = new DispatchResourceToDisasterCommand(
                disasterId,
                request.ResourceId,
                request.TeamId,
                request.Quantity,
                request.Notes);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => NoContent(),
                Problem);
        }


        [HttpPost(ApiEndpoints.Disasters.ConsumeResource)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [EndpointSummary("Consume dispatched disaster resource.")]
        [EndpointDescription("Consumes a specific quantity from a resource that was previously dispatched to the disaster operation.")]
        [EndpointName("ConsumeDisasterResource")]
        public async Task<IActionResult> ConsumeResource([FromRoute] Guid disasterId, [FromRoute] Guid resourceId, [FromBody] ConsumeDisasterResourceRequest request,CancellationToken cancellationToken)
        {
            var command = new ConsumeDisasterResourceCommand(
                disasterId,
                resourceId,
                request.Quantity);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => NoContent(),
                Problem);
        }



        [HttpPost(ApiEndpoints.Disasters.ReturnResource)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [EndpointSummary("Return disaster resource.")]
        [EndpointDescription("Returns a specific quantity of a dispatched resource back to the assigned team's inventory.")]
        [EndpointName("ReturnDisasterResource")]
        public async Task<IActionResult> ReturnResource([FromRoute] Guid disasterId, [FromRoute] Guid resourceId, [FromBody] ReturnDisasterResourceRequest request, CancellationToken cancellationToken)
        {
            var command = new ReturnDisasterResourceCommand(
                disasterId,
                resourceId,
                request.Quantity);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => NoContent(),
                Problem);
        }



        [HttpPost(ApiEndpoints.Disasters.MarkResourceDamaged)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [EndpointSummary("Mark disaster resource as damaged.")]
        [EndpointDescription("Marks a specific quantity of a dispatched disaster resource as damaged during the disaster operation.")]
        [EndpointName("MarkDisasterResourceDamaged")]
        public async Task<IActionResult> MarkResourceDamage([FromRoute] Guid disasterId, [FromRoute] Guid resourceId, [FromBody] MarkDisasterResourceDamagedRequest request, CancellationToken cancellationToken)
        {
            var command = new MarkDisasterResourceDamagedCommand(
                disasterId,
                resourceId,
                request.Quantity);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => NoContent(),
                Problem);
        }


        [HttpPost(ApiEndpoints.Disasters.Resolve)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [EndpointSummary("resolve disaster")]
        [EndpointDescription("Marks a disaster as resolved when all field operations are completed.")]
        [EndpointName("ResolveDisaster")]
        public async Task<IActionResult> Resolve([FromRoute] Guid disasterId,CancellationToken cancellationToken)
        {
            var command = new ResolveDisasterCommand(disasterId);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => NoContent(),
                Problem);
        }


        [HttpPost(ApiEndpoints.Disasters.AddAffectedPersons)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [EndpointSummary("add affected persons")]
        [EndpointDescription("Adds a list of affected people recorded after the disaster is resolved.")]
        [EndpointName("AddAffectedPersons")]
        public async Task<IActionResult> AddAffectedPersons([FromRoute] Guid disasterId, [FromBody] AddAffectedPersonsRequest request,CancellationToken cancellationToken)
        {
            var command = new AddAffectedPersonsCommand(
                disasterId,
                request.Persons.ToDtos());

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => NoContent(),
                Problem);
        }



        [HttpPut(ApiEndpoints.Disasters.UpdateAffectedPersons)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [EndpointSummary("update affected person")]
        [EndpointDescription("Updates details of an affected person linked to a disaster.")]
        [EndpointName("UpdateAffectedPerson")]
        public async Task<IActionResult> Update([FromRoute] Guid disasterId, [FromRoute] Guid affectedPersonId, [FromBody] UpdateAffectedPersonRequest request, CancellationToken cancellationToken)
        {
            if (!HealthStatus.TryFromName(request.Status, true, out var status))
                return Problem(AffectedPersonErrors.InvalidStatus);


            var command = new UpdateAffectedPersonCommand(
                disasterId,
                affectedPersonId,
                request.Name,
                request.Age,
                request.Phone,
                status,
                request.Notes);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => NoContent(),
                Problem);
        }



        [HttpPost(ApiEndpoints.Disasters.Close)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [EndpointSummary("close disaster")]
        [EndpointDescription("Closes a resolved disaster and finalizes all operations.")]
        [EndpointName("CloseDisaster")]
        public async Task<IActionResult> Close([FromRoute] Guid disasterId,CancellationToken cancellationToken)
        {
            var command = new CloseDisasterCommand(disasterId);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => NoContent(),
                Problem);
        }


        [HttpPost(ApiEndpoints.Disasters.GenerateReport)]
        [ProducesResponseType(typeof(GenerateDisasterReportResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [EndpointSummary("Generate disaster report.")]
        [EndpointDescription("Generates a detailed PDF report for a closed disaster, uploads it to cloud storage, " +"stores the report metadata, and returns the generated report URL.")]
        [EndpointName("GenerateDisasterReport")]
        public async Task<IActionResult> GenerateReport([FromRoute] Guid disasterId,  CancellationToken cancellationToken)
        {
            var command = new GenerateDisasterReportCommand(disasterId);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                v=> base.Ok(new GenerateDisasterReportResponse(v.ReportUrl)),
                Problem);
        }




        // [Authorize(Roles = ApplicationRole.Admin)]
        [HttpGet(ApiEndpoints.Disasters.GetAll)]
        [ProducesResponseType(typeof(PagedResponse<DisasterResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Retrieves disasters.")]
        [EndpointDescription("Returns a paginated list of disasters with optional filtering by status, type, location, and time.")]
        [EndpointName("GetDisasters")]
        public async Task<IActionResult> GetDisasters([FromQuery] GetDisastersFilter filter, [FromQuery] PagedRequest pagedRequest, CancellationToken cancellationToken)
        {
            DisasterType? type = null;

            if (filter.Type != null)
            {

                if (!DisasterType.TryFromName(filter.Type, true, out var parsed))
                    return Problem(DisasterErrors.InvalidType);
                type = parsed;
            }

            DisasterStatus? status = null;

            if (filter.Status != null)
            {

                if (!DisasterStatus.TryFromName(filter.Status, true, out var parsed))
                    return Problem(DisasterErrors.InvalidStatus);
                status = parsed;
            }



            var query = new GetDisastersQuery(pagedRequest.Page, pagedRequest.PageSize, filter.City, filter.Province, type , status , filter.From , filter.To);

            var res = await _sender.Send(query, cancellationToken);

            return
                 Ok(new PagedResponse<DisasterResponse>(res.PageNumber, res.PageSize, res.TotalPages, res.TotalCount, res.Items.ToResponses()));
        }


        // [Authorize(Roles = ApplicationRole.Admin)]
        [HttpGet(ApiEndpoints.Disasters.GetById)]
        [ProducesResponseType(typeof(DisasterDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Retrieves disaster by id .")]
        [EndpointDescription("Returns a detailed disaster")]
        [EndpointName("GetDisasterById")]
        public async Task<IActionResult> GetDisasterById([FromRoute] Guid disasterId , CancellationToken cancellationToken)
        {
            var command = new GetDisasterByIdQuery(disasterId);

            var res = await _sender.Send(command, cancellationToken);

            if (res is null)
                return Problem(ApplicationErrors.DisasterNotFound);

            return Ok(res.ToResponse()); 
        }

        [HttpGet(ApiEndpoints.Disasters.GetTimeline)]
        [ProducesResponseType(typeof(PaginatedList<DisasterTimelineDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Retrieves disaster timeline by disaster id")]
        [EndpointDescription("Returns a paginated and sorted list of all timeline events related to a disaster including status changes, assignments, and system events")]
        [EndpointName("GetDisasterTimeline")]
        public async Task<IActionResult> GetTimeline( Guid disasterId, [FromQuery] PagedRequest pagedRequest,[FromQuery] GetTimeLineFilter filter ,CancellationToken cancellationToken)
        {
            var query = new GetDisasterTimelineQuery(
                disasterId,
                pagedRequest.Page,
                pagedRequest.PageSize,
                filter.EventType,
                MapSort(filter.SortDirection)
            );

            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }




        [HttpPost(ApiEndpoints.Disasters.EvaluateVolunteer)]
        [EndpointSummary("Evaluates a volunteer performance in a disaster.")]
        [EndpointDescription("Allows a team leader to evaluate a volunteer based on multiple performance metrics such as commitment, safety, teamwork, and initiative.")]
        [EndpointName("EvaluateVolunteer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EvaluateVolunteer( Guid disasterId, Guid volunteerId, EvaluateVolunteerRequest request, CancellationToken cancellationToken)
        {
            var command = new EvaluateVolunteerCommand(
                disasterId,
                volunteerId,
                request.CommitmentScore,
                request.SkillScore,
                request.SafetyScore,
                request.TeamWorkScore,
                request.InitiativeScore,
                request.Notes);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => NoContent(),
                Problem);
        }

        private static TimelineSortDirection MapSort(string sort)
        {
            return sort?.ToLower() switch
            {
                "asc" => TimelineSortDirection.Oldest,
                "desc" => TimelineSortDirection.Newest,
                _ => TimelineSortDirection.Newest
            };
        }

    }
}

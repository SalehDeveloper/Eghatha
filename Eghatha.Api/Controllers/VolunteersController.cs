using Eghatha.Api.Mappers;
using Eghatha.Application.Common.Errors;
using Eghatha.Application.Features.Teams.Queries.GetTeamById;
using Eghatha.Application.Features.Teams.Queries.GetTeams;
using Eghatha.Application.Features.Volunteers.Commands.AddVolunteerEquipment;
using Eghatha.Application.Features.Volunteers.Commands.CreateVolunteer;
using Eghatha.Application.Features.Volunteers.Commands.DecreaseEquipmentQuantity;
using Eghatha.Application.Features.Volunteers.Commands.IncreaseVolunteerEquipment;
using Eghatha.Application.Features.Volunteers.Commands.RemoveVolunteerEquipment;
using Eghatha.Application.Features.Volunteers.Commands.UpdateEquipmentStatus;
using Eghatha.Application.Features.Volunteers.Commands.UpdateLocation;
using Eghatha.Application.Features.Volunteers.Commands.UpdateStatus;
using Eghatha.Application.Features.Volunteers.Commands.UpdateVolunteerEquipment;
using Eghatha.Application.Features.Volunteers.Queries.GetAll;
using Eghatha.Application.Features.Volunteers.Queries.GetById;
using Eghatha.Application.Features.Volunteers.Queries.GetEquipments;
using Eghatha.Contract.Shared;
using Eghatha.Contract.Teams.Requests;
using Eghatha.Contract.Teams.Responses;
using Eghatha.Contract.Volunteers.Requests;
using Eghatha.Contract.Volunteers.Responses;
using Eghatha.Domain.Teams;
using Eghatha.Domain.Volunteers;
using Eghatha.Domain.Volunteers.Equipments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eghatha.Api.Controllers
{
    public class VolunteersController : ApiController
    {
        public VolunteersController(ISender sender) : base(sender)
        {
        }



        [HttpPost(ApiEndpoints.Volunteers.Create)]
        [ProducesResponseType(typeof(CreateVolunteerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [EndpointSummary("Creates a new volunteer.")]
        [EndpointDescription("Creates a new volunteer")]
        [EndpointName("CreateVolunteer")]
        public async Task<IActionResult> CreateVolunteer([FromForm] CreateVolunteerRequest request, CancellationToken cancellationToken)
        {

            if (!VolunteerSpeciality.TryFromName(request.Speciality, true, out var speciality))
                return Problem(VolunteerErrors.SpecialityInvalid);

            var command = new CreateVolunteerCommand(request.FirstName,
                request.LastName,
                request.Email,
                request.PhoneNumber,
                request.Password,
                request.Photo,
                speciality,
                request.Latitude,
                request.Longitude,
                request.YearsOfExperience,
                request.Cv);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                v => base.Ok(v),
                Problem);
        }


        // [Authorize(Roles = ApplicationRole.Admin)]
        [HttpGet(ApiEndpoints.Volunteers.GetAll)]
        [ProducesResponseType(typeof(PagedResponse<VolunteerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Retrieves volunteers.")]
        [EndpointDescription("Returns a paginated list of volunteers with optional filtering by status, speciality, location, and search term.")]
        [EndpointName("GetVolunteers")]
        public async Task<IActionResult> Getvolunteers([FromQuery] GetVolunteersFilter filter, [FromQuery] PagedRequest pagedRequest, CancellationToken cancellationToken)
        {
            VolunteerSpeciality? speciality = null;

            if (filter.Speciality != null)
            {

                if (!VolunteerSpeciality.TryFromName(filter.Speciality, true, out var parsed))
                    return Problem(VolunteerErrors.SpecialityInvalid);
                speciality = parsed;
            }

            VolunteerStatus? status = null;

            if (filter.Status != null)
            {

                if (!VolunteerStatus.TryFromName(filter.Status, true, out var parsed))
                    return Problem(VolunteerErrors.StatusInvalid);
                status = parsed;
            }



            var query = new GetAllVolunteersQuery(pagedRequest.Page, pagedRequest.PageSize, filter.SearchTerm, status, speciality, filter.Province, filter.City);

            var res = await _sender.Send(query, cancellationToken);

            return
                 Ok(new PagedResponse<VolunteerResponse>(res.PageNumber, res.PageSize, res.TotalPages, res.TotalCount, res.Items.ToResponses()));
        }

        // [Authorize(Roles = ApplicationRole.Admin)]
        [HttpGet(ApiEndpoints.Volunteers.GetById)]
        [ProducesResponseType(typeof(VolunteerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Retrieves a volunteer by ID.")]
        [EndpointDescription("Returns detailed information about a specific volunteer.")]
        [EndpointName("GetVolunteerById")]
        public async Task<IActionResult> GetById([FromRoute] Guid volunteerid, CancellationToken cancellationToken)
        {

            var query = new GetVolunteerByIdQuery(volunteerid);

            var res = await _sender.Send(query, cancellationToken);

            if (res is null)
                return Problem(ApplicationErrors.TeamNotFound);

            return
                 Ok(res.ToResponse());
        }




        // [Authorize(Roles = ApplicationRole.Admin)]
        [HttpPut(ApiEndpoints.Volunteers.volunteerBusy)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [EndpointSummary("Set volunteer status to busy.")]
        [EndpointDescription("Updates the status of a specific volunteer to busy.")]
        [EndpointName("SetVolunteerBusy")]
        public async Task<IActionResult> VolunteerBusy(
            [FromRoute] Guid volunteerId,
            CancellationToken cancellationToken)
        {
            var command = new UpdateVolunteerStatusCommand(
                volunteerId,
               VolunteerStatus.Busy
            );

            var result = await _sender.Send(command, cancellationToken);
            return result.Match(
                 _ => base.NoContent(),
                 Problem);
        }

        // [Authorize(Roles = ApplicationRole.Admin)]
        [HttpPut(ApiEndpoints.Volunteers.volunteerAvailable)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [EndpointSummary("Updates volunteer status to available.")]
        [EndpointDescription("Updates the status of a specific volunteer to available.")]
        [EndpointName("SetVolunteerAvailable")]
        public async Task<IActionResult> VolunteerAvailable(
            [FromRoute] Guid volunteerId,
            CancellationToken cancellationToken)
        {
            var command = new UpdateVolunteerStatusCommand(
                volunteerId,
               VolunteerStatus.Available
            );

            var result = await _sender.Send(command, cancellationToken);
            return result.Match(
                 _ => base.NoContent(),
                 Problem);
        }



        // [Authorize(Roles = ApplicationRole.Admin)]
        [HttpPut(ApiEndpoints.Volunteers.volunteerUnAvailable)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [EndpointSummary("Updates volunteer status to unavailable.")]
        [EndpointDescription("Updates the status of a specific volunteer to unavailable.")]
        [EndpointName("SetVolunteerUnAvailable")]
        public async Task<IActionResult> VolunteerUnAvailable(
            [FromRoute] Guid volunteerId,
            CancellationToken cancellationToken)
        {
            var command = new UpdateVolunteerStatusCommand(
                volunteerId,
               VolunteerStatus.UnAvailable
            );

            var result = await _sender.Send(command, cancellationToken);
            return result.Match(
                 _ => base.NoContent(),
                 Problem);
        }



        //[Authorize(Roles = ApplicationRole.Volunteer)]
        [HttpPut(ApiEndpoints.Volunteers.UpdateLocation)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [EndpointSummary("Updates volunteer location.")]
        [EndpointDescription("Updates the geographic location of a volunteer.")]
        [EndpointName("UpdateVolunteerLocation")]
        public async Task<IActionResult> UpdateLocation([FromRoute] Guid volunteerId,[FromBody] UpdateVolunteerLocationRequest request,CancellationToken cancellationToken)
        {
            var command = new UpdateVolunteerLocationCommand(
                volunteerId,
                request.Latitude,
                request.Longitude
            );

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                          _ => base.NoContent(),
                          Problem);
        }


        [Authorize(Roles = ApplicationRole.Volunteer)]
        [HttpPost(ApiEndpoints.Volunteers.AddEquipment)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Create a new volunteer equipment.")]
        [EndpointDescription("Creates a new equipment entry for a volunteer.")]
        [EndpointName("CreateVolunteerEquipment")]
        public async Task<IActionResult> CreateVolunteerEquipment([FromRoute] Guid volunteerId,[FromBody] CreateVolunteerEquipmentRequest request, CancellationToken cancellationToken)
        {
            EquipmentCategory? category = null;

            if (request.Category != null)
            {

                if (!EquipmentCategory.TryFromName(request.Category, true, out var parsed))
                    return Problem(EquipmentErrors.UnSupportedCategory);
                category = parsed;
            }
            var command = new AddVolunteerEquipmentCommand(
                volunteerId,
                request.Name,
                category,
                request.Quantity
            );
            var result = await _sender.Send(command, cancellationToken);
            return result.Match(
                _ => base.NoContent(),
                Problem
            );
        }






        [Authorize(Roles = ApplicationRole.Volunteer)]
        [HttpPut(ApiEndpoints.Volunteers.IncreaseEquipmentQuantity)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Increase equipment quantity.")]
        [EndpointDescription("Increases the quantity of a specific equipment for a volunteer.")]
        [EndpointName("IncreaseVolunteerEquipmentQuantity")]
        public async Task<IActionResult> IncreaseEquipmentQuantity( [FromRoute] Guid volunteerId,[FromRoute] Guid equipmentId, [FromBody] ChangeEquipmentQuantityRequest request, CancellationToken cancellationToken)
        {
            var command = new IncreaseVolunteerEquipmentQuantityCommand(
                volunteerId,
                equipmentId,
                request.Quantity
            );

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => NoContent(),
                Problem
            );
        }


        [Authorize(Roles = ApplicationRole.Volunteer)]
        [HttpPut(ApiEndpoints.Volunteers.DecreaseEquipmentQuantity)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Decrease equipment quantity.")]
        [EndpointDescription("Decreases the quantity of a specific equipment for a volunteer.")]
        [EndpointName("DecreaseVolunteerEquipmentQuantity")]
        public async Task<IActionResult> DecreaseEquipmentQuantity( [FromRoute] Guid volunteerId,[FromRoute] Guid equipmentId,[FromBody] ChangeEquipmentQuantityRequest request, CancellationToken cancellationToken)
        {
            var command = new DecreaseVolunteerEquipmentQuantityCommand(
                volunteerId,
                equipmentId,
                request.Quantity
            );

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => NoContent(),
                Problem
            );
        }



        [Authorize(Roles = ApplicationRole.Volunteer)]
        [HttpPut(ApiEndpoints.Volunteers.UpdateEquipment)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Update equipment.")]
        [EndpointDescription("Updates equipment details such as name, category, status, or quantity. Only provided fields will be updated.")]
        [EndpointName("UpdateVolunteerEquipment")]
        public async Task<IActionResult> UpdateEquipment( [FromRoute] Guid volunteerId, [FromRoute] Guid equipmentId,[FromBody] UpdateEquipmentRequest request, CancellationToken cancellationToken)
        {

            EquipmentStatus? status = null;

            if (request.Status is not null)
            {
                if (!EquipmentStatus.TryFromName(request.Status, true, out var parsed))
                    return Problem(EquipmentErrors.InvalidStatus);

                status = parsed;
            }

            EquipmentCategory? category = null;

            if (request.Category is not null)
            {
                if (!EquipmentCategory.TryFromName(request.Status, true, out var parsed))
                    return Problem(EquipmentErrors.UnSupportedCategory);

                category = parsed;
            }

            var command = new UpdateVolunteerEquipmentCommand(
                volunteerId,
                equipmentId,
                request.Name,
                category,
                status,
                request.Quantity
            );

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => NoContent(),
                Problem
            );
        }



        [Authorize(Roles = ApplicationRole.Volunteer)]
        [HttpPut(ApiEndpoints.Volunteers.EquipmentValid)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Update equipment status to Valid.")]
        [EndpointDescription("Updates the status of a specific equipment to valid.")]
        [EndpointName("EquipemntValidStatus")]
        public async Task<IActionResult> EquipmentValid([FromRoute] Guid volunteerId, [FromRoute] Guid equipmentId, CancellationToken cancellationToken)
        {
            var command = new UpdateVolunteerEquipmentStatusCommand(
                volunteerId,
                equipmentId,
                EquipmentStatus.Valid
            );

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => NoContent(),
                Problem
            );
        }


        [Authorize(Roles = ApplicationRole.Volunteer)]
        [HttpPut(ApiEndpoints.Volunteers.EquipmentInValid)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Update equipment status to in-valid.")]
        [EndpointDescription("Updates the status of a specific equipment to in-valid.")]
        [EndpointName("EquipemntInValidStatus")]
        public async Task<IActionResult> EquipmentInValid([FromRoute] Guid volunteerId,[FromRoute] Guid equipmentId, CancellationToken cancellationToken)
        {
            var command = new UpdateVolunteerEquipmentStatusCommand(
                volunteerId,
                equipmentId,
                EquipmentStatus.InValid
            );

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => NoContent(),
                Problem
            );
        }


        [Authorize(Roles = ApplicationRole.Volunteer)]
        [HttpDelete(ApiEndpoints.Volunteers.RemoveEquipment)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Remove equipment.")]
        [EndpointDescription("Removes a specific equipment from the volunteer.")]
        [EndpointName("RemoveVolunteerEquipment")]
        public async Task<IActionResult> RemoveEquipment([FromRoute] Guid volunteerId,[FromRoute] Guid equipmentId, CancellationToken cancellationToken)
        {
            var command = new RemoveVolunteerEquipmentCommand(
                volunteerId,
                equipmentId
            );

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => NoContent(),
                Problem
            );
        }





     //   [Authorize(Roles = ApplicationRole.Admin)]
        [HttpGet(ApiEndpoints.Volunteers.GetVolunteerEquipments)]
        [ProducesResponseType(typeof(PagedResponse<VolunteerEquipmentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Get volunteer equipments.")]
        [EndpointDescription("Retrieves a list of equipments for a specific volunteer.")]
        [EndpointName("GetVolunteerEquipments")]
        public async Task<IActionResult> GetVolunteerEquipments([FromRoute] Guid volunteerId, [FromQuery] GetVolunteerEquipmentsFilter filter, [FromQuery] PagedRequest pagedRequest, CancellationToken cancellationToken)
        {
            EquipmentCategory? category = null;

            if (filter.Category != null)
            {

                if (!EquipmentCategory.TryFromName(filter.Category, true, out var parsed))
                    return Problem(EquipmentErrors.UnSupportedCategory);
                category = parsed;
            }

            var query = new GetVolunteerEquipmentsQuery(volunteerId, pagedRequest.Page, pagedRequest.PageSize, category);

            var res = await _sender.Send(query, cancellationToken);

            return
                  Ok(new PagedResponse<VolunteerEquipmentResponse>(res.PageNumber, res.PageSize, res.TotalPages, res.TotalCount, res.Items.ToResponses()));

        }
    }
}

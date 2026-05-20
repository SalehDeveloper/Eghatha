using Eghatha.Application.Features.Disasters.Queries.GetAffectedPesonHealthStatuses;
using Eghatha.Application.Features.Disasters.Queries.GetDisasterStatuses;
using Eghatha.Application.Features.Disasters.Queries.GetDisasterTypes;
using Eghatha.Application.Features.Teams.Queries.GetResourceStatuses;
using Eghatha.Application.Features.Teams.Queries.GetResourceTypes;
using Eghatha.Application.Features.Teams.Queries.GetTeamMemberStatuses;
using Eghatha.Application.Features.Teams.Queries.GetTeamSpecialities;
using Eghatha.Application.Features.Teams.Queries.GetTeamStatuses;
using Eghatha.Application.Features.VolunteerRegisterations.Queries.GetRegistrationStatuses;
using Eghatha.Application.Features.Volunteers.Queries.GetEquipmentCategories;
using Eghatha.Application.Features.Volunteers.Queries.GetEquipmentStatuses;
using Eghatha.Application.Features.Volunteers.Queries.GetVolunteerSpecialities;
using Eghatha.Application.Features.Volunteers.Queries.GetVolunteerStatuses;
using Eghatha.Domain.Teams.TeamMembers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Eghatha.Api.Controllers
{
    public class ReferenceDataController : ApiController
    {
        public ReferenceDataController(ISender sender) : base(sender)
        {
        }

        [HttpGet(ApiEndpoints.ReferenceData.GetDisasterTypes)]
        [ProducesResponseType(typeof(IReadOnlyList<DisasterTypeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [EndpointSummary("Gets all disaster types.")]
        [EndpointDescription("Returns a list of all supported disaster types that can be used when creating or filtering disasters.")]
        [EndpointName("GetDisasterTypes")]
        public async Task<IActionResult> GetDisasterTypes(CancellationToken cancellationToken)
        {
            var query = new GetDisasterTypesQuery();

            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }



        [HttpGet(ApiEndpoints.ReferenceData.GetDisasterStatuses)]
        [ProducesResponseType(typeof(IReadOnlyList<DisasterStatusResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [EndpointSummary("Gets all disaster statuses.")]
        [EndpointDescription("Returns a list of all supported disaster statuses that can be used when filtering disasters.")]
        [EndpointName("GetDisasterStatuses")]
        public async Task<IActionResult> GetDisasterStatuses(CancellationToken cancellationToken)
        {
            var query = new GetDisasterStatusesQuery();

            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }


        [HttpGet(ApiEndpoints.ReferenceData.GetHealthStatuses)]
        [ProducesResponseType(typeof(IReadOnlyList<AffectedPersonHealthStatusResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [EndpointSummary("Gets all health statuses.")]
        [EndpointDescription("Returns a list of all supported health statuses that can be used when filtering , createing affected persons.")]
        [EndpointName("GetAffectedPersonHealthStatuses")]
        public async Task<IActionResult> GetAffectedPersonHealthStatuses(CancellationToken cancellationToken)
        {
            var query = new GetHealthStatusesQuery();

            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpGet(ApiEndpoints.ReferenceData.GetTeamSpecialities)]
        [ProducesResponseType(typeof(IReadOnlyList<TeamSpecialityResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [EndpointSummary("Gets all team specialities.")]
        [EndpointDescription("Returns a list of all supported team specialities  that can be used when filtering , createing teams.")]
        [EndpointName("GetAffectedPersonHealthStatuses")]
        public async Task<IActionResult> GetTeamSpecialities(CancellationToken cancellationToken)
        {
            var query = new GetTeamSpecialitiesQuery();

            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }


        [HttpGet(ApiEndpoints.ReferenceData.GetTeamStatuses)]
        [ProducesResponseType(typeof(IReadOnlyList<TeamStatusResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [EndpointSummary("Gets all team statuses.")]
        [EndpointDescription("Returns a list of all supported team statuses  that can be used when filtering teams.")]
        [EndpointName("GetTeamStatuses")]
        public async Task<IActionResult> GetTeamStatuses(CancellationToken cancellationToken)
        {
            var query = new GetTeamStatusesQuery();

            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }


        [HttpGet(ApiEndpoints.ReferenceData.GetTeamMemberStatuses)]
        [ProducesResponseType(typeof(IReadOnlyList<TeamMemberStatus>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [EndpointSummary("Gets all team-member statuses.")]
        [EndpointDescription("Returns a list of all supported team-member statuses  that can be used when filtering team-members.")]
        [EndpointName("GetTeamMemberStatuses")]
        public async Task<IActionResult> GetTeamMemberStatuses(CancellationToken cancellationToken)
        {
            var query = new GetTeamMemberStatusesQuery();

            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }



        [HttpGet(ApiEndpoints.ReferenceData.GetResourceStatuses)]
        [ProducesResponseType(typeof(IReadOnlyList<ResourceStatusResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [EndpointSummary("Gets all resource statuses.")]
        [EndpointDescription("Returns a list of all supported rsouce statuses  that can be used when filtering , creating resource.")]
        [EndpointName("GetResourceStatuses")]
        public async Task<IActionResult> GetResourceStatuses(CancellationToken cancellationToken)
        {
            var query = new GetResourceStatusesQuery();

            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }


        [HttpGet(ApiEndpoints.ReferenceData.GetResourceTypes)]
        [ProducesResponseType(typeof(IReadOnlyList<ResourceTypeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [EndpointSummary("Gets all resource types.")]
        [EndpointDescription("Returns a list of all supported rsouce types  that can be used when filtering , creating resource.")]
        [EndpointName("GetResourceTypes")]
        public async Task<IActionResult> GetResourceTypes(CancellationToken cancellationToken)
        {
            var query = new GetResourceTypesQuery();

            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }


        [HttpGet(ApiEndpoints.ReferenceData.GetRegistrationStatuses)]
        [ProducesResponseType(typeof(IReadOnlyList<RegistrationStatusResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [EndpointSummary("Gets all registration statuses.")]
        [EndpointDescription("Returns a list of all registration statuses used for volunteer registration workflow.")]
        [EndpointName("GetRegistrationStatuses")]
        public async Task<IActionResult> GetRegistrationStatuses(CancellationToken cancellationToken)
        {
            var query = new GetRegistrationStatusesQuery();

            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }


        [HttpGet(ApiEndpoints.ReferenceData.GetVolunteerStatuses)]
        [ProducesResponseType(typeof(IReadOnlyList<VolunteerStatusResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [EndpointSummary("Gets all volunteer statuses.")]
        [EndpointDescription("Returns a list of all volunteer statuses such as available, busy, and under review.")]
        [EndpointName("GetVolunteerStatuses")]
        public async Task<IActionResult> GetVolunteerStatuses(CancellationToken cancellationToken)
        {
            var query = new GetVolunteerStatusesQuery();

            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }


        [HttpGet(ApiEndpoints.ReferenceData.GetVolunteerSpecialities)]
        [ProducesResponseType(typeof(IReadOnlyList<VolunteerSpecialityResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [EndpointSummary("Gets all volunteer specialities.")]
        [EndpointDescription("Returns a list of all volunteer specialities used for assigning skills to volunteers.")]
        [EndpointName("GetVolunteerSpecialities")]
        public async Task<IActionResult> GetVolunteerSpecialities(CancellationToken cancellationToken)
        {
            var query = new GetVolunteerSpecialitiesQuery();

            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpGet(ApiEndpoints.ReferenceData.GetEquipmentCategories)]
        [ProducesResponseType(typeof(IReadOnlyList<EquipmentCategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [EndpointSummary("Gets all equipment categories.")]
        [EndpointDescription("Returns a list of all equipment categories used to classify disaster response equipment.")]
        [EndpointName("GetEquipmentCategories")]
        public async Task<IActionResult> GetEquipmentCategories(CancellationToken cancellationToken)
        {
            var query = new GetEquipmentCategoriesQuery();

            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }

   
        
        [HttpGet(ApiEndpoints.ReferenceData.GetEquipmentStatuses)]
        [ProducesResponseType(typeof(IReadOnlyList<EquipmentStatusResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [EndpointSummary("Gets all equipment statuses.")]
        [EndpointDescription("Returns a list of all equipment statuses such as valid, invalid, in maintenance, and damaged.")]
        [EndpointName("GetEquipmentStatuses")]
        public async Task<IActionResult> GetEquipmentStatuses(CancellationToken cancellationToken)
        {
            var query = new GetEquipmentStatusesQuery();

            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }
    }
}

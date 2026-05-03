using Eghatha.Api.Mappers;
using Eghatha.Application.Common.Errors;
using Eghatha.Application.Features.Teams.Commands.AddTeamMember;
using Eghatha.Application.Features.Teams.Commands.AddTeamResource;
using Eghatha.Application.Features.Teams.Commands.ChangeTeamLeader;
using Eghatha.Application.Features.Teams.Commands.CreateTeam;
using Eghatha.Application.Features.Teams.Commands.DeActivateTeamMember;
using Eghatha.Application.Features.Teams.Commands.DecreaseTeamReosurce;
using Eghatha.Application.Features.Teams.Commands.IncreaseTeamResource;
using Eghatha.Application.Features.Teams.Commands.UpdateLiveTeamLocation;
using Eghatha.Application.Features.Teams.Commands.UpdateTeam;
using Eghatha.Application.Features.Teams.Commands.UpdateTeamStatus;
using Eghatha.Application.Features.Teams.Queries.GetTeamById;
using Eghatha.Application.Features.Teams.Queries.GetTeamMembers;
using Eghatha.Application.Features.Teams.Queries.GetTeamResources;
using Eghatha.Application.Features.Teams.Queries.GetTeams;
using Eghatha.Contract.Accounts.Responses;
using Eghatha.Contract.Shared;
using Eghatha.Contract.Teams.Requests;
using Eghatha.Contract.Teams.Responses;
using Eghatha.Domain.Teams;
using Eghatha.Domain.Teams.Resources;
using Eghatha.Domain.Teams.TeamMembers;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eghatha.Api.Controllers
{
    public class TeamsController : ApiController
    {
        public TeamsController(ISender sender) : base(sender)
        {
        }


        //[Authorize(ApplicationRole.Admin)]
        [HttpPost(ApiEndpoints.Teams.Create)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [EndpointSummary("Creates a new team.")]
        [EndpointDescription("Creates a new team with the specified name, speciality, and location. Requires Admin privileges.")]
        [EndpointName("CreateTeam")]
        public async Task<IActionResult> CreateTeam(CreateTeamRequest request, CancellationToken cancellationToken)
        {
            if (!TeamSpeciality.TryFromName(request.Speciality, true, out var speciality))
                return Problem(TeamErrors.InvalidSpeciality);

            var command = new CreateTeamCommand(request.Name, speciality,  request.Latitude, request.Longitude);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                v => base.CreatedAtAction(
                    nameof(GetById),
                    new { teamid = v} ,
                    null
                   ),
                Problem);


        }


        //[Authorize(ApplicationRole.Admin)]
        [HttpPatch(ApiEndpoints.Teams.Update)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Updates a team.")]
        [EndpointDescription("Updates team details such as name, speciality, province, and city. Only provided fields will be updated.")]
        [EndpointName("UpdateTeam")]
        public async Task<IActionResult> UpdateTeam([FromRoute] Guid teamid, [FromBody] UpdateTeamRequest request, CancellationToken cancellationToken)
        {

            TeamSpeciality? speciality = null;

            if (request.Speciality is not null)
            {
                if (!TeamSpeciality.TryFromName(request.Speciality, true, out var parsed))
                    return Problem(TeamErrors.InvalidSpeciality);

                speciality = parsed;
            }

            var command = new UpdateTeamCommand(
                teamid,
                request.Name,
                speciality,
                request.Province,
                request.City
            );

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => base.NoContent(),
                Problem
            );
        }


        //[Authorize(ApplicationRole.Admin)]
        [HttpPost(ApiEndpoints.Teams.Activate)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Activates a team.")]
        [EndpointDescription("Sets the team status to Active. Requires Admin privileges.")]
        [EndpointName("ActivateTeam")]
        public async Task<IActionResult> ActivateTeam([FromRoute] Guid teamid, CancellationToken cancellationToken)
        {


            var command = new UpdateTeamStatusCommand(teamid, TeamStatus.Active);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => base.NoContent(),
                Problem);
        }


        //[Authorize(ApplicationRole.Admin)]
        [HttpPost(ApiEndpoints.Teams.DeActivate)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Deactivates a team.")]
        [EndpointDescription("Sets the team status to Inactive. Requires Admin privileges.")]
        [EndpointName("DeactivateTeam")]

        public async Task<IActionResult> DeactivateTeam([FromRoute] Guid teamid, CancellationToken cancellationToken)
        {


            var command = new UpdateTeamStatusCommand(teamid, TeamStatus.Inactive);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => base.NoContent(),
                Problem);
        }


        //[Authorize(Roles = ApplicationRole.TeamMember)]
        //[Authorize(Policy =ApplicationPolicies.CanUpdateTeamLocation)]
        [HttpPatch(ApiEndpoints.Teams.UpdateLiveLocation)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Updates team live location.")]
        [EndpointDescription("Updates the current geographic location (latitude and longitude) of the team. Typically used for real-time tracking.")]
        [EndpointName("UpdateTeamLiveLocation")]
        public async Task<IActionResult> UpdateLiveLocation([FromRoute] Guid teamid, [FromBody] UpdateTeamLocation request, CancellationToken cancellationToken)
        {
            var command = new UpdateLiveTeamLocationCommand(teamid, request.Latitude, request.Longitude);
            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => base.NoContent(),
                Problem);

        }




         [Authorize(Roles = ApplicationRole.Admin)]
        [HttpPost(ApiEndpoints.Teams.AddMemeber)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Adds a member to a team.")]
        [EndpointDescription("Creates and assigns a new member to the specified team. Optionally sets the member as team leader.")]
        [EndpointName("AddTeamMember")]
        public async Task<IActionResult> AddTeamMember([FromRoute] Guid teamid, [FromBody] AddTeamMemberRequest request, CancellationToken cancellationToken)
        {
            var command = new AddTeamMemberCommand(teamid, request.FirstName, request.LastName, request.Email, request.PhoneNumber, request.Photo, request.JobTitle, request.IsLeader);


            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => base.NoContent(),
                Problem);
        }


        // [Authorize(Roles = ApplicationRole.Admin)]
        [HttpPost(ApiEndpoints.Teams.DeactivateMember)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Deactivates a team member.")]
        [EndpointDescription("Sets the specified team member status to Inactive.")]
        [EndpointName("DeactivateTeamMember")]
        public async Task<IActionResult> DeactivateTeamMember([FromRoute] Guid teamid, [FromRoute] Guid memberid, CancellationToken cancellationToken)
        {

            var command = new UpdateTeamMemberStatusCommand(teamid, memberid, TeamMemberStatus.Inactive);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => base.NoContent(),
                Problem);
        }

        // [Authorize(Roles = ApplicationRole.Admin)]
        [HttpPost(ApiEndpoints.Teams.ActivateMember)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Activates a team member.")]
        [EndpointDescription("Sets the specified team member status to Active.")]
        [EndpointName("ActivateTeamMember")]
        public async Task<IActionResult> ActivateTeamMember([FromRoute] Guid teamid, [FromRoute] Guid memberid, CancellationToken cancellationToken)
        {

            var command = new UpdateTeamMemberStatusCommand(teamid, memberid, TeamMemberStatus.Active);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => base.NoContent(),
                Problem);
        }

        // [Authorize(Roles = ApplicationRole.Admin)]
        [HttpPost(ApiEndpoints.Teams.OnMissionMemberStatus)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Marks a member as on mission.")]
        [EndpointDescription("Updates the team member status to OnMission.")]
        [EndpointName("SetMemberOnMission")]
        public async Task<IActionResult> MemberOnMission([FromRoute] Guid teamid, [FromRoute] Guid memberid, CancellationToken cancellationToken)
        {

            var command = new UpdateTeamMemberStatusCommand(teamid, memberid, TeamMemberStatus.OnMission);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => base.NoContent(),
                Problem);
        }

        // [Authorize(Roles = ApplicationRole.Admin)]
        [HttpPost(ApiEndpoints.Teams.OffDutyMemberStatus)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Marks a member as off duty.")]
        [EndpointDescription("Updates the team member status to OffDuty.")]
        [EndpointName("SetMemberOffDuty")]
        public async Task<IActionResult> MemberOffDuty([FromRoute] Guid teamid, [FromRoute] Guid memberid, CancellationToken cancellationToken)
        {

            var command = new UpdateTeamMemberStatusCommand(teamid, memberid, TeamMemberStatus.OffDuty);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => base.NoContent(),
                Problem);
        }



        // [Authorize(Roles = ApplicationRole.Admin)]
        [HttpPost(ApiEndpoints.Teams.ChangeLeader)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Changes team leader.")]
        [EndpointDescription("Assigns a new leader for the team from existing team members.")]
        [EndpointName("ChangeTeamLeader")]
        public async Task<IActionResult> ChangeLeader([FromRoute] Guid teamid, [FromRoute] Guid memberid, CancellationToken cancellationToken)
        {
            var command = new ChangeTeamLeaderCommand(teamid, memberid);
            var result = await _sender.Send(command, cancellationToken);
            return result.Match(
                _ => base.NoContent(),
                Problem);
        }


        // [Authorize(Roles = ApplicationRole.Admin)]
        [HttpPost(ApiEndpoints.Teams.AddResource)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Adds a resource to a team.")]
        [EndpointDescription("Creates a new resource entry for the team with the specified type and quantity.")]
        [EndpointName("AddTeamResource")]
        public async Task<IActionResult> AddResource([FromRoute] Guid teamid, [FromBody] AddTeamResourceRequest request, CancellationToken cancellationToken)
        {
            ResourceType? type = null;

            if (request.resourceType is not null)
            {
                if (!ResourceType.TryFromName(request.resourceType, true, out var parsed))
                    return Problem(TeamErrors.InvalidSpeciality);

                type = parsed;
            }


            var command = new AddTeamResourceCommand(teamid, type, request.Quantity);

            var res = await _sender.Send(command, cancellationToken);

            return res.Match(
              _ => base.NoContent(),
              Problem);
        }


        // [Authorize(Roles = ApplicationRole.Admin)]
        [HttpPost(ApiEndpoints.Teams.IncreaseResourceQuantity)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Increases resource quantity.")]
        [EndpointDescription("Increases the quantity of an existing team resource.")]
        [EndpointName("IncreaseResourceQuantity")]
        public async Task<IActionResult> IncreaseResourceQuantity([FromRoute] Guid teamid, [FromRoute] Guid resourceid, [FromBody] int quantity, CancellationToken cancellationToken)
        {
            var command = new IncreaseTeamResourceCommand(teamid, resourceid, quantity);
            var res = await _sender.Send(command, cancellationToken);
            return res.Match(
              _ => base.NoContent(),
              Problem);

        }


        // [Authorize(Roles = ApplicationRole.Admin)]
        [HttpPost(ApiEndpoints.Teams.DecreaseResourceQuantity)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Decreases resource quantity.")]
        [EndpointDescription("Decreases the quantity of an existing team resource.")]
        [EndpointName("DecreaseResourceQuantity")]
        public async Task<IActionResult> DecreaseResourceQuantity([FromRoute] Guid teamid, [FromRoute] Guid resourceid, [FromBody] int quantity, CancellationToken cancellationToken)
        {
            var command = new DecreaseTeamResourceCommand(teamid, resourceid, quantity);
            var res = await _sender.Send(command, cancellationToken);
            return res.Match(
              _ => base.NoContent(),
              Problem);

        }




        // [Authorize(Roles = ApplicationRole.Admin)]
        [HttpGet(ApiEndpoints.Teams.GetAll)]
        [ProducesResponseType(typeof(PagedResponse<TeamResponse>) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Retrieves teams.")]
        [EndpointDescription("Returns a paginated list of teams with optional filtering by status, speciality, location, and search term.")]
        [EndpointName("GetTeams")]
        public async Task<IActionResult> GetTeams([FromQuery] GetTeamsFilter filter ,  [FromQuery] PagedRequest pagedRequest , CancellationToken cancellationToken )
        {
            TeamSpeciality? speciality = null;

            if (filter.Speciality != null)
            {

                if (!TeamSpeciality.TryFromName(filter.Speciality, true, out var parsed))
                    return Problem(TeamErrors.InvalidSpeciality);
                speciality = parsed;
            }

            TeamStatus? status = null;

            if (filter.Status != null)
            {

                if (!TeamStatus.TryFromName(filter.Status, true, out var parsed))
                    return Problem(TeamErrors.InvalidSpeciality);
                status = parsed;
            }



            var query = new GetTeamsQuery(pagedRequest.Page, pagedRequest.PageSize, filter.SearchTerm, status, speciality, filter.Province, filter.City);

            var res =  await _sender.Send(query, cancellationToken);

            return
                 Ok(new PagedResponse<TeamResponse>(res.PageNumber, res.PageSize, res.TotalPages, res.TotalCount, res.Items.ToResponses()));
        }


        // [Authorize(Roles = ApplicationRole.Admin)]
        [HttpGet(ApiEndpoints.Teams.GetById)]
        [ProducesResponseType(typeof(TeamResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Retrieves a team by ID.")]
        [EndpointDescription("Returns detailed information about a specific team.")]
        [EndpointName("GetTeamById")]
        public async Task<IActionResult> GetById([FromRoute] Guid teamid ,CancellationToken cancellationToken)
        {
            
            var query = new GetTeamByIdQuery(teamid);

            var res = await _sender.Send(query, cancellationToken);

            if (res is null)
                return Problem(ApplicationErrors.TeamNotFound);

            return
                 Ok( res.ToResponse());
        }


        // [Authorize(Roles = ApplicationRole.Admin)]
        [HttpGet(ApiEndpoints.Teams.GetTeamMembers)]
        [ProducesResponseType(typeof(PagedResponse<TeamMemberResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Retrieves team members.")]
        [EndpointDescription("Returns a paginated list of members belonging to a specific team with optional filtering.")]
        [EndpointName("GetTeamMembers")]
        public async Task<IActionResult> GetTeamMembers([FromRoute] Guid teamid ,[FromQuery] PagedRequest pagedRequest , [FromQuery] GetTeamMembersFilter filter, CancellationToken cancellationToken)
        {
            TeamMemberStatus? status = null;

            if (filter.MemberStatus != null)
            {

                if (!TeamMemberStatus.TryFromName(filter.MemberStatus, true, out var parsed))
                    return Problem(TeamErrors.InvalidSpeciality);
                status = parsed;
            }

            var query = new GetTeamMembersQuery(teamid, pagedRequest.Page, pagedRequest.PageSize, filter.SearchTerm, status);

            var res = await _sender.Send(query, cancellationToken);

            return Ok(new PagedResponse<TeamMemberResponse>(res.PageNumber, res.PageSize, res.TotalPages, res.TotalCount, res.Items.ToResponses()));

        }



        // [Authorize(Roles = ApplicationRole.Admin)]
        [HttpGet(ApiEndpoints.Teams.GetTeamResources)]
        [ProducesResponseType(typeof(PagedResponse<TeamResourceResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Retrieves team resources.")]
        [EndpointDescription("Returns a paginated list of resources assigned to a specific team.")]
        [EndpointName("GetTeamResources")]
        public async Task<IActionResult> GetTeamResources([FromRoute] Guid teamid, [FromQuery] PagedRequest pagedRequest, [FromQuery] GetTeamResourcesFilter filter, CancellationToken cancellationToken)
        {
            ResourceType? type = null;

            if (filter.Type != null)
            {

                if (!ResourceType.TryFromName(filter.Type, true, out var parsed))
                    return Problem(TeamErrors.InvalidSpeciality);
                type = parsed;
            }

            var query = new GetTeamResourcesQuery(teamid, pagedRequest.Page, pagedRequest.PageSize, type);

            var res = await _sender.Send(query, cancellationToken);

            return Ok(new PagedResponse<TeamResourceResponse>(res.PageNumber, res.PageSize, res.TotalPages, res.TotalCount, res.Items.ToResponses()));
        }

    }
}
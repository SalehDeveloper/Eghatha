using Eghatha.Application.Features.Notifications.Commands.MarkAllAsRead;
using Eghatha.Application.Features.Notifications.Commands.MarkNotificationAsRead;
using Eghatha.Application.Features.Notifications.Dtos;
using Eghatha.Application.Features.Notifications.Queries.GetById;
using Eghatha.Application.Features.Notifications.Queries.GetNotifications;
using Eghatha.Application.Features.Notifications.Queries.GetUnreadCount;
using Eghatha.Contract.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eghatha.Api.Controllers
{
    public class NotificationController : ApiController
    {
        public NotificationController(ISender sender) : base(sender)
        {
        }


        [Authorize]
        [HttpGet(ApiEndpoints.Notifications.GetAll)]
        [ProducesResponseType(typeof(PagedResponse<NotificationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [EndpointSummary("Get user notifications.")]
        [EndpointDescription("Returns paginated notifications for the current logged-in user.")]
        [EndpointName("GetNotifications")]
        public async Task<IActionResult> GetNotifications([FromQuery] PagedRequest request,CancellationToken cancellationToken)
        {
            var query = new GetNotificationsQuery(request.Page, request.PageSize);

            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }


        [Authorize]
        [HttpGet(ApiEndpoints.Notifications.GetUnreadCount)]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [EndpointSummary("Get unread notifications count.")]
        public async Task<IActionResult> GetUnreadCount(CancellationToken cancellationToken)
        {
            var query = new GetUnreadCountQuery();

            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }



        [Authorize]
        [HttpPost(ApiEndpoints.Notifications.MarkAsRead)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [EndpointSummary("Mark notification as read.")]
        [EndpointDescription("Marks a single notification as read for the current user.")]
        public async Task<IActionResult> MarkAsRead( Guid notificationid,CancellationToken cancellationToken)
        {
            var command = new MarkNotificationAsReadCommand(notificationid);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => NoContent(),
                Problem);
        }

        [Authorize]
        [HttpPost(ApiEndpoints.Notifications.MarkAllAsRead)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [EndpointSummary("Mark all notifications as read.")]
        public async Task<IActionResult> MarkAllAsRead( CancellationToken cancellationToken)
        {
            var command = new MarkAllNotificationsAsReadCommand();

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                _ => NoContent(),
                Problem);
        }


        [Authorize]
        [HttpGet(ApiEndpoints.Notifications.GetById)]
        [ProducesResponseType(typeof(NotificationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [EndpointSummary("Get notification by id.")]
        public async Task<IActionResult> GetById(  Guid notificationid,CancellationToken cancellationToken)
        {
            var query = new GetNotificationByIdQuery(notificationid);

            var result = await _sender.Send(query, cancellationToken);

            return result.Match(Ok, Problem);
        }

    }
}

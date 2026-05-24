using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Notifications.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Notifications.Queries.GetNotifications
{
    public sealed record GetNotificationsQuery(int Page, int PageSize) : IRequest<PaginatedList<NotificationDto>>;


}

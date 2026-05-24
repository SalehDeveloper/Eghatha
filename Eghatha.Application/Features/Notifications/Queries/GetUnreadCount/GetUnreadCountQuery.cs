using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Notifications.Queries.GetUnreadCount
{
    public sealed record GetUnreadCountQuery : IRequest<int>;
}

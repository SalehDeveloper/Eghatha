using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Disasters.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Queries.GetTimeLine
{
    public sealed record GetDisasterTimelineQuery(
       Guid DisasterId,
       int Page,
       int PageSize,
       string? EventType,
       TimelineSortDirection Sort
   ):IRequest<PaginatedList<DisasterTimelineDto>>;

    public enum TimelineSortDirection
    {
        Newest,
        Oldest
    }
}

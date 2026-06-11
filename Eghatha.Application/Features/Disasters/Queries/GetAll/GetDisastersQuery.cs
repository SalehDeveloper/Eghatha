using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Disasters.Dtos;
using Eghatha.Domain.Disasters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Queries.GetAll
{
    public record GetDisastersQuery(
     int Page,
     int PageSize,
     string? City,
     string? Province,
     string? Type,
     string? Status,
     DateTimeOffset? From,
     DateTimeOffset? To
 ) : ICachedQuery<PaginatedList<DisasterDto>>
    {
        public string[] Tags => ["disasters"];

        public TimeSpan Expiration => TimeSpan.FromMinutes(5);

        public string CachKey => $"disasters:p={Page}:ps={PageSize}:city={City ?? "-"}:province={Province ?? "-"}:type={Type?? "-"}:status={Status?? "-"}:from={From}:to={To}";
    }
}

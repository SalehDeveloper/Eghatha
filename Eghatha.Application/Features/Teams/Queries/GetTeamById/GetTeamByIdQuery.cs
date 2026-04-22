using Eghatha.Application.Features.Teams.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Queries.GetTeamById
{
    public record GetTeamByIdQuery(Guid TeamId) : ICachedQuery<TeamDto>
    {
        public string CachKey => $"team_{TeamId}";

        public string[] Tags => ["teams"];

        public TimeSpan Expiration => TimeSpan.FromMinutes(10);
    }
}

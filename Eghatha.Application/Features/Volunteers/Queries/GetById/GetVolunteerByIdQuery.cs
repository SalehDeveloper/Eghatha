using Eghatha.Application.Features.Volunteers.Dtos;
using Eghatha.Domain.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Volunteers.Queries.GetById
{
    public record GetVolunteerByIdQuery(Guid Id) : ICachedQuery<VolunteerDto>
    {
        public string CachKey => $"volunteer{Id}";

        public string[] Tags => ["volunteers"];

        public TimeSpan Expiration => TimeSpan.FromMinutes(10);
    }
}

using Eghatha.Application.Features.Disasters.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Queries.GetById
{
    public record GetDisasterByIdQuery(Guid DisasterId)
     : ICachedQuery<DisasterDetailsDto>
    {
      
        public string[] Tags => ["disasters"];
        public TimeSpan Expiration => TimeSpan.FromMinutes(10);

        public string CachKey => $"disaster:{DisasterId}";
    }
}

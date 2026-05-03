using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Services
{
    public interface IGeocodingService
    {
        Task<LocationResult> ResolveAsync(double lat, double lng, CancellationToken cancellationToken);

        public record LocationResult(string City, string Province);
    }
}

using Eghatha.Application.Common.Services;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static Eghatha.Application.Common.Services.IGeocodingService;

namespace Eghatha.Infastructure.Services
{
    public class OpenStreetMapService : IGeocodingService
    {
        private readonly HttpClient _http;

        public OpenStreetMapService(HttpClient http)
        {
            _http = http;
        }

        public async Task<LocationResult> ResolveAsync(double lat, double lng, CancellationToken ct)
        {
            var url = $"reverse?lat={lat}&lon={lng}&format=json&accept-language=en";

            var response = await _http.GetFromJsonAsync<OsmResponse>(url, ct);

            var address = response?.Address;

            return new LocationResult(
                address?.City ?? address?.Town ?? address?.Village ?? "Unknown",
                address?.State ?? address?.Region ?? "Unknown"
            );
        }
    }
    public class OsmResponse
    {
        public OsmAddress Address { get; set; }
    }

    public class OsmAddress
    {
        public string City { get; set; }
        public string Town { get; set; }
        public string Village { get; set; }
        public string State { get; set; }
        public string Region { get; set; }
    }
}

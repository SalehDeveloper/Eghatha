using Eghatha.Application.Common.Models;
using Eghatha.Application.Common.Services;
using Eghatha.Domain.Shared.ValueObjects;
using System.Net.Http.Json;

namespace Eghatha.Infastructure.Services
{
    public sealed class OpenRouteServiceRoutingService : IRoutingService
    {
        private readonly HttpClient _httpClient;

        public OpenRouteServiceRoutingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyList<RouteResult>> CalculateAsync(
            GeoLocation source,
            IReadOnlyList<RouteDestination> destinations,
            CancellationToken cancellationToken)
        {
            var locations = new List<double[]>
        {
            new[] { source.Longitude, source.Latitude }
        };

            locations.AddRange(destinations.Select(d =>
                new[] { d.Location.Longitude, d.Location.Latitude }));

            var request = new
            {
                locations,
                sources = new[] { 0 },
                destinations = Enumerable.Range(1, destinations.Count).ToArray(),
                metrics = new[] { "distance", "duration" }
            };

            var response = await _httpClient.PostAsJsonAsync(
                "v2/matrix/driving-car",
                request,
                cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
     $"Status: {(int)response.StatusCode} {response.StatusCode}\n" +
     $"Response: {responseBody}\n" +
     $"Request locations: {System.Text.Json.JsonSerializer.Serialize(locations)}");
            }

            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<MatrixResponse>(cancellationToken: cancellationToken);

            if (result is null)
                return [];

            var output = new List<RouteResult>();
            for (int i = 0; i < destinations.Count; i++)
            {
                var distance = result.distances[0][i];
                var duration = result.durations[0][i];

                if (distance is null || duration is null)
                    continue;

                output.Add(new RouteResult(
                    destinations[i].EntityId,
                    distance.Value / 1000,
                    duration.Value / 60));
            }

            return output;
        }
    }
}

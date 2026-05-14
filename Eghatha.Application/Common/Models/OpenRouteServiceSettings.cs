namespace Eghatha.Application.Common.Models
{
    public sealed class OpenRouteServiceSettings
    {
        public const string SectionName = "OpenRouteService";

        public string ApiKey { get; set; } = string.Empty;
    }
}

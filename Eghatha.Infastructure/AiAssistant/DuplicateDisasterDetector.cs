using Eghatha.Application.Features.AiAssistant;
using Eghatha.Application.Features.Disasters.Dtos;
using ErrorOr;
using Google.GenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Eghatha.Application.Features.AiAssistant.IDuplicateDisasterDetector;

namespace Eghatha.Infastructure.AiAssistant
{
    public class DuplicateDisasterDetector : IDuplicateDisasterDetector
    {
        private readonly Client _client;
        private readonly ILogger<DuplicateDisasterDetector> _logger;

        private const string SystemPrompt = """
            You compare a newly reported disaster against a list of candidate disasters
            that were reported recently, nearby, and of the same type.
            Decide whether the new report describes the SAME real-world incident as one
            of the candidates (duplicate/spam report), or a genuinely different incident.
            Respond with ONLY a JSON object, no markdown, no text outside the JSON:
            {"isLikelyDuplicate": bool, "matchedDisasterId": "<guid or null>", "confidence": 0.0-1.0, "reasoning": "short reason"}
            If unsure, prefer isLikelyDuplicate=false — a missed real report is worse than a redundant one.
            """;

        public DuplicateDisasterDetector(IConfiguration config, ILogger<DuplicateDisasterDetector> logger)
        {
            var apiKey = config["Gemini:ApiKey"]
                ?? Environment.GetEnvironmentVariable("GEMINI_API_KEY")
                ?? throw new InvalidOperationException("Missing Gemini API key.");
            _client = new Client(apiKey: apiKey);
            _logger = logger;
        }

        public async Task<ErrorOr<DuplicateCheckResult>> CheckAsync(
            NewDisasterReportDto newReport, List<DuplicateCandidateDto> candidates, CancellationToken ct)
        {
            var prompt = $"""
                {SystemPrompt}

                New report:
                {JsonSerializer.Serialize(newReport)}

                Candidates:
                {JsonSerializer.Serialize(candidates)}
                """;

            var response = await _client.Models.GenerateContentAsync(
                model: "gemini-3.1-flash-lite", contents: prompt, cancellationToken: ct);

            var text = StripCodeFences(response.Text ?? string.Empty);

            try
            {
                var parsed = JsonSerializer.Deserialize<DuplicateCheckResult>(
                    text, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (parsed is null)
                    return Error.Failure("AiParseError", "Could not parse duplicate check response.");

                _logger.LogInformation(
                    "Spam check for {DisasterId}: IsDuplicate={IsDuplicate} Confidence={Confidence}",
                    newReport.DisasterId, parsed.IsLikelyDuplicate, parsed.Confidence);

                return parsed;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse AI duplicate-check response: {Raw}", text);
                return Error.Failure("AiParseError", "Could not parse duplicate check response.");
            }
        }

        private static string StripCodeFences(string text)
        {
            var t = text.Trim();
            if (t.StartsWith("```"))
            {
                var firstNewline = t.IndexOf('\n');
                t = firstNewline >= 0 ? t[(firstNewline + 1)..] : t;
                var fenceEnd = t.LastIndexOf("```", StringComparison.Ordinal);
                if (fenceEnd >= 0) t = t[..fenceEnd];
            }
            return t.Trim();
        }
    }
}

using Eghatha.Application.Features.AiAssistant;
using ErrorOr;
using Google.GenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.AiAssistant
{
    public class AiQueryAssistant : IAiQueryAssistant
    {
        private readonly Client _client;
        private readonly ILogger<AiQueryAssistant> _logger;

        private static readonly string SqlSystemPrompt = $"""
    You are a T-SQL generator for an admin reporting tool over a disaster-relief system.

    You may ONLY query these views, exactly as listed:

    {ReportSchema.Definition}

    Rules, no exceptions:
    - Output ONE single SELECT statement and nothing else — no explanation, no markdown, no code fences.
    - Never reference any table or view not listed above.
    - Never use INSERT, UPDATE, DELETE, DROP, ALTER, EXEC, or any DDL/DML.
    - Never use comments (-- or /*).
    - Always include a reasonable TOP limit.
    - Every selected column MUST have an explicit alias, especially aggregates and
      expressions. Example: SELECT COUNT(Id) AS TeamCount FROM report.Teams
      — never SELECT COUNT(Id) FROM report.Teams.
    - If the question cannot be answered from the views above, output exactly: NO_QUERY
    """;

        private const string SummarySystemPrompt = """
        You are an assistant summarizing database query results for an admin of a
        disaster-relief coordination system. You will be given the admin's original
        question and a JSON array of result rows. Answer the question in plain,
        concise language based only on the provided rows. If the array is empty,
        say plainly that no matching records were found — do not guess or invent data.
        """;

        public AiQueryAssistant(IConfiguration config, ILogger<AiQueryAssistant> logger)
        {
            var apiKey = config["Gemini:ApiKey"]
                ?? Environment.GetEnvironmentVariable("GEMINI_API_KEY")
                ?? throw new InvalidOperationException("Missing Gemini API key.");

            _client = new Client(apiKey: apiKey);
            _logger = logger;
        }

        public async Task<ErrorOr<string>> GenerateSqlAsync(string question, CancellationToken ct)
        {
            var prompt = $"""
            {SqlSystemPrompt}

            User question:
            {question}
            """;

            var response = await _client.Models.GenerateContentAsync(
      model: "gemini-3.1-flash-lite",
      contents: prompt,
      cancellationToken: ct);

            var text = (response.Text ?? "NO_QUERY").Trim();

            _logger.LogInformation(
                "AI generated SQL for question. Question={Question} Sql={Sql}",
                question, text);

            if (text.Equals("NO_QUERY", StringComparison.OrdinalIgnoreCase))
              return Error.Conflict("NoQuery", "The question cannot be answered from the available report views.");

            return StripCodeFences(text);
        }

        public async Task<string> SummarizeResultsAsync(
            string question,
            string resultsJson,
            CancellationToken ct)
        {
            var prompt = $"""
            {SummarySystemPrompt}

            Question: {question}

            Results (JSON):
            {resultsJson}
            """;
            var response = await _client.Models.GenerateContentAsync(
                model: "gemini-3.1-flash-lite",
                contents: prompt,
                cancellationToken: ct);

            return (response.Text ?? "No summary available.").Trim();
        }

        private static string StripCodeFences(string text)
        {
            var t = text.Trim();

            if (t.StartsWith("```"))
            {
                var firstNewline = t.IndexOf('\n');
                t = firstNewline >= 0 ? t[(firstNewline + 1)..] : t;

                var fenceEnd = t.LastIndexOf("```", StringComparison.Ordinal);
                if (fenceEnd >= 0)
                    t = t[..fenceEnd];
            }

            return t.Trim();
        }
    }
}


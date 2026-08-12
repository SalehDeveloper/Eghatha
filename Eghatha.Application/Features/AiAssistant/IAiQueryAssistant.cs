using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.AiAssistant
{
    public interface  IAiQueryAssistant
    {
        /// Turns the admin's natural-language question into one SELECT statement
        /// against the report.* views only.
        Task<string> GenerateSqlAsync(string question, CancellationToken ct);

        /// Turns the question + the raw query results into a natural-language answer.
        Task<string> SummarizeResultsAsync(string question, string resultsJson, CancellationToken ct);
    }
}


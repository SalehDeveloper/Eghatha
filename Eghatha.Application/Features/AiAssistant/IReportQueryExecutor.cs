using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.AiAssistant
{
    public interface  IReportQueryExecutor
    {
        /// Validates and executes sql against the report.* views (ai_reader connection).
        /// Throws InvalidOperationException if the query fails validation.
        Task<IReadOnlyList<Dictionary<string, object?>>> ExecuteAsync(string sql, CancellationToken ct);
    }
}

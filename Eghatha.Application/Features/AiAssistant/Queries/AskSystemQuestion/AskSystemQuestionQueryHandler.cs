using MediatR;
using System.Text.Json;

namespace Eghatha.Application.Features.AiAssistant.Queries.AskSystemQuestion
{
    public class AskSystemQuestionQueryHandler
    : IRequestHandler<AskSystemQuestionQuery, AskSystemQuestionResult>
    {
        private readonly IAiQueryAssistant _ai;
        private readonly IReportQueryExecutor _executor;
       

        public AskSystemQuestionQueryHandler(
            IAiQueryAssistant ai,
            IReportQueryExecutor executor)
         
        {
            _ai = ai;
            _executor = executor;
          
        }

        public async Task<AskSystemQuestionResult> Handle(
            AskSystemQuestionQuery request, CancellationToken ct)
        {
            // 1. NL question -> SQL (AI, restricted to report.* views per ReportSchema)
            var sql = await _ai.GenerateSqlAsync(request.Question, ct);

            // 2. Validate + execute against the ai_reader connection.
            //    Throws InvalidOperationException if SqlGuard rejects it — let it bubble,
            //    the controller will translate it into a 400.
            var rows = await _executor.ExecuteAsync(sql, ct);

            // 3. Rows -> natural-language answer (AI again)
            var resultsJson = JsonSerializer.Serialize(rows);
            var answer = await _ai.SummarizeResultsAsync(request.Question, resultsJson, ct);

          

            return new AskSystemQuestionResult(answer, sql, rows.Count);
        }
    }

}

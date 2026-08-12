using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.AiAssistant.Queries.AskSystemQuestion
{
    public record AskSystemQuestionQuery(string Question) : IRequest<AskSystemQuestionResult>;
    public record AskSystemQuestionResult(
    string Answer,
    string GeneratedSql,
    int RowCount);

}

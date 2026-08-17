using Eghatha.Application.Features.Disasters.Dtos;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.AiAssistant
{
    public interface IDuplicateDisasterDetector
    {
        Task<ErrorOr<DuplicateCheckResult>> CheckAsync(
            NewDisasterReportDto newReport,
            List<DuplicateCandidateDto> candidates,
            CancellationToken ct);


        public record DuplicateCheckResult(
            bool IsLikelyDuplicate,
            Guid? MatchedDisasterId,
            double Confidence,
            string Reasoning);
    }
}

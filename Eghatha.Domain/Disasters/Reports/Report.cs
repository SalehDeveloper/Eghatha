using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Shared.Errors;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Disasters.Reports
{
    public sealed class Report : AuditableEntity
    {
        public Guid DisasterId { get; private set; }

        public string Summary { get; private set; }

        public string PdfUrl { get; private set; }

        public DateTimeOffset IssuedAt { get; private set; }

        private Report() { }

        private Report(
            Guid id,
            Guid disasterId,
            string summary,
            string pdfUrl,
            DateTimeOffset issuedAt) : base(id)
        {
            DisasterId = disasterId;
            Summary = summary;
            PdfUrl = pdfUrl;
            IssuedAt = issuedAt;
        }

        public static ErrorOr<Report> Create(
            Guid id,
            Guid disasterId,
            string summary,
            string pdfUrl,
            DateTimeOffset issuedAt)
        {
            if (id == Guid.Empty)
                return DomainErrors.IdMustBeProvided(nameof(Report));

            if (disasterId == Guid.Empty)
                return DomainErrors.IdMustBeProvided(nameof(Disaster));

            if (string.IsNullOrWhiteSpace(summary))
                return Error.Validation("Report.Summary", "Summary is required.");

            if (string.IsNullOrWhiteSpace(pdfUrl))
                return Error.Validation("Report.PdfUrl", "PdfUrl is required.");

            return new Report(id, disasterId, summary, pdfUrl, issuedAt);
        }
    }
}

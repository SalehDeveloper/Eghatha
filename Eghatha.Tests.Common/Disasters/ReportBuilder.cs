using Eghatha.Domain.Disasters.Reports;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Tests.Common.Disasters
{
    public sealed class ReportBuilder
    {
        private Guid _id = Guid.NewGuid();
        private Guid _disasterId = Guid.NewGuid();
        private string _summary = "Test report summary";
        private string _pdfUrl = "https://example.com/reports/test.pdf";
        private DateTimeOffset _issuedAt = new(2026, 2, 1, 8, 0, 0, TimeSpan.Zero);

        public static ReportBuilder Valid() => new();

        public ReportBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public ReportBuilder WithDisasterId(Guid disasterId)
        {
            _disasterId = disasterId;
            return this;
        }

        public ReportBuilder WithSummary(string summary)
        {
            _summary = summary;
            return this;
        }

        public ReportBuilder WithPdfUrl(string pdfUrl)
        {
            _pdfUrl = pdfUrl;
            return this;
        }

        public ReportBuilder WithIssuedAt(DateTimeOffset issuedAt)
        {
            _issuedAt = issuedAt;
            return this;
        }

        public ErrorOr<Report> Build() =>
            Report.Create(_id, _disasterId, _summary, _pdfUrl, _issuedAt);

        public Report BuildValid() => Build().Value;
    }
}

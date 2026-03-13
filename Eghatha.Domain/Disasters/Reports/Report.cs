using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Disasters.Reports
{
    public sealed record Report
    {
        public string Summary { get; private set ; }

        public string Teams { get; private set; }

        public string Resources { get; private set; }   

        public string AffectedPersons { get; private set; } 

        public DateTimeOffset IssuedAt { get; private set; }

        private Report() { }

        private Report(string summary, string teams, string resources, string affectedPersons , DateTimeOffset issuedAt)
        {
            Summary = summary;
            Teams = teams;
            Resources = resources;
            AffectedPersons = affectedPersons;
            IssuedAt = issuedAt;
        }


        public static ErrorOr<Report> Create(string summary, string teams, string resources, string affectedPersons , DateTimeOffset issuedAt)
        {
            if (string.IsNullOrEmpty(summary))
            {
                return Error.Validation("Report.Summary", "Summary is required.");
            }
            if (string.IsNullOrEmpty(teams))
            {
                return Error.Validation("Report.Teams", "Teams information is required.");
            }
            if (string.IsNullOrEmpty(resources))
            {
                return Error.Validation("Report.Resources", "Resources information is required.");
            }
            if (string.IsNullOrEmpty(affectedPersons))
            {
                return Error.Validation("Report.AffectedPersons", "Affected persons information is required.");
            }
            return new Report(summary, teams, resources, affectedPersons , issuedAt);
            
            
        }
    }
}

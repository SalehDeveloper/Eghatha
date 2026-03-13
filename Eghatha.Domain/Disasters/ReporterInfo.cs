using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Disasters
{
    public sealed record  ReporterInfo
    {
        public string Name { get; }
        public string IdNumber { get; }
        public string Contact { get; }

        private ReporterInfo(string name, string idNumber, string contact)
        {
            Name = name;
            IdNumber = idNumber;
            Contact = contact;
        }

        public static ErrorOr<ReporterInfo> Create(
            string name,
            string idNumber,
            string contact)
        {
            if (string.IsNullOrWhiteSpace(name))
                return DisasterErrors.ReporterNameRequired;

            if (string.IsNullOrWhiteSpace(contact))
                return DisasterErrors.ReporterContactRequired;

            if (string.IsNullOrWhiteSpace(idNumber))
                return DisasterErrors.ReporterIdRequired;

            return new ReporterInfo(name, idNumber, contact);
        }
    }
}

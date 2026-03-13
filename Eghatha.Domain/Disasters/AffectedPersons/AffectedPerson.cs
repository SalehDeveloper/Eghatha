using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Shared.Errors;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Disasters.AffectedPersons
{
    public sealed class AffectedPerson : AuditableEntity
    {
        public Guid DisasterId { get; private set; }
        public string Name { get; private set; }
        public int Age { get; private set; }
        public string Phone { get; private set; }
        public HealthStatus Status { get; private set; }
        public string? Notes { get; private set; }

        private AffectedPerson()
        {

        }

        private AffectedPerson(
            Guid id,
            Guid disasterId,
            string name,
            int age,
            string phone,
            HealthStatus status,
            string? notes)
            : base(id)
        {
            DisasterId = disasterId;
            Name = name;
            Age = age;
            Phone = phone;
            Status = status;
            Notes = notes;
        }

        public static ErrorOr<AffectedPerson> Create(
            Guid id,
            Guid disasterId,
            string name,
            int age,
            string phone,
            HealthStatus status,
            string? notes)
        {
            if (id == Guid.Empty)
                return DomainErrors.IdMustBeProvided("AffectedPerson");

            if (disasterId == Guid.Empty)
                return DomainErrors.IdMustBeProvided("Disaster");

            if (string.IsNullOrWhiteSpace(name))
                return AffectedPersonErrors.NameRequired;

            if (age < 0)
                return AffectedPersonErrors.InvalidAge;

            var affectedPerson = new AffectedPerson(
                id,
                disasterId,
                name,
                age,
                phone,
                status,
                notes);
           
            
            return affectedPerson;
        }

        public ErrorOr<Updated> Update(
            string? name = null,
            int? age = null,
            string? phone=null,
            HealthStatus? status=null,
            string? notes = null)
        {
            if (name is not null && string.IsNullOrWhiteSpace(name))
                return AffectedPersonErrors.NameRequired;
            if (age is not null &&  age < 0)
                return AffectedPersonErrors.InvalidAge;
         
            
            Name = name?? Name;
            Age = age?? Age;
            Phone = phone?? Phone;
            Status = status?? Status;
            Notes = notes?? Notes;

            return Result.Updated;
        }
    }
}
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Shared.Errors;
using Eghatha.Domain.VolunteerRegisterations.Events;
using Eghatha.Domain.Volunteers;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.VolunteerRegisterations
{
    public sealed class VolunteerRegisteration : AuditableEntity
    {
        public Guid VolunteerId { get; private set; }

        public RegisterationStatus Status { get; private set; }
        public DateTimeOffset RequestedAt { get; private set; }

        public DateTimeOffset? ReviewedAt { get; private set; }

        public Guid? ReviewedByAdminId { get; private set; }

        public string? RejectionReason { get; private set; }

        private VolunteerRegisteration()
        {


        }

        private VolunteerRegisteration(
            Guid id,
            Guid volunteerId,
            DateTimeOffset requestedAt
                )
            : base(id)
        {
            VolunteerId = volunteerId;
            RequestedAt = requestedAt;
            Status = RegisterationStatus.Pending;
        }


        public static  ErrorOr<VolunteerRegisteration> Create(Guid volunteerId, DateTimeOffset requestedAt)
        {
            if (volunteerId == Guid.Empty)
            {
                return DomainErrors.IdMustBeProvided(nameof(Volunteer));
            }

            var registeration = new VolunteerRegisteration(Guid.NewGuid(), volunteerId, requestedAt);

            registeration.AddDomainEvent(new VolunteerRegisterationCreated(registeration.Id, registeration.VolunteerId, requestedAt));

            return registeration;
        }


        public ErrorOr<Updated> Approve(DateTimeOffset reviewedAt, Guid reviewedById)
        {
            if (Status != RegisterationStatus.Pending)
                return VolunteerRegisterationErrors.AlreadyProcessed;

            Status = RegisterationStatus.Approved;
            ReviewedAt = reviewedAt;
            ReviewedByAdminId = reviewedById;

            AddDomainEvent(new VolunteerRegisterationApproved(VolunteerId));
            return Result.Updated;
        }

        public ErrorOr<Updated> Reject(DateTimeOffset reviewedAt, Guid reviewedById , string reason)
        {
            if (Status != RegisterationStatus.Pending)
                return VolunteerRegisterationErrors.AlreadyProcessed;

            Status = RegisterationStatus.Rejected;
            ReviewedAt = reviewedAt;
            ReviewedByAdminId = reviewedById;
            RejectionReason = reason;

            AddDomainEvent(new VolunteerRegisterationRejected(VolunteerId, reason));
            return Result.Updated;

        }
    }
}
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Shared.Errors;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Disasters.DisasterVolunteers
{
    public sealed class DisasterVolunteer : AuditableEntity
    {
        private DisasterVolunteer()
        {
            
        }

        public Guid VolunteerId { get; private set; }
        public Guid DisasterId { get; private set; }

        public EvaluationScores? EvaluationScores { get; private set; }
        public string? Notes { get; private set; } 

        public DateTimeOffset? EvaluatedAt { get; private set; }

        public Guid? EvaluatedByLeaderId { get; private set; }


        private DisasterVolunteer(
            Guid id ,
            Guid volunteerId , 
            Guid disasterId,
            EvaluationScores? evaluation,
            string? notes,
            DateTimeOffset? evaluatedAt, 
            Guid? evaluatedByLeaderId )
            :base(id)
        {
            VolunteerId = volunteerId;
            DisasterId = disasterId;
            EvaluationScores = evaluation;
            Notes = notes ?? string.Empty;
            EvaluatedAt = evaluatedAt;
            EvaluatedByLeaderId = evaluatedByLeaderId;

        }

        public static ErrorOr<DisasterVolunteer> Create(
            Guid id,
            Guid volunteerId,
            Guid disasterId)
        {
            if (id == Guid.Empty)
                return DomainErrors.IdMustBeProvided(nameof(DisasterVolunteer));

            if (volunteerId == Guid.Empty)
                return DomainErrors.IdMustBeProvided("Volunteer");

            if (disasterId == Guid.Empty)
                return DomainErrors.IdMustBeProvided("DisasterId");


            return new DisasterVolunteer(id, volunteerId, disasterId, null, null , null , null ); 
        }

        public ErrorOr<Updated> Evaluate(
            EvaluationScores evaluation , 
            string? notes , 
            Guid leaderId , 
            DateTimeOffset evaluatedAt)
        {

            if (leaderId == Guid.Empty)
                return DomainErrors.IdMustBeProvided("Leader");

            EvaluationScores = evaluation;
            Notes = notes;
            EvaluatedByLeaderId = leaderId;
            EvaluatedAt= evaluatedAt;

            return Result.Updated;
            
        }

    }
}

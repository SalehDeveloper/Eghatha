using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Disasters.AffectedPersons;
using Eghatha.Domain.Disasters.DisasterResources;
using Eghatha.Domain.Disasters.DisasterVolunteers;
using Eghatha.Domain.Disasters.Reports;
using Eghatha.Domain.Shared.Errors;
using Eghatha.Domain.Shared.ValueObjects;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Disasters
{
    public sealed class Disaster : AuditableEntity
    {
        public string Title { get; private set; }

        public string Description { get; private set; }

        public GeoLocation Location { get; private set; }

        public DateTimeOffset StartTime { get; private set; }

        public DateTimeOffset? EndTime { get; private set; }

        public DisasterStatus Status { get; private set; }

        public DisasterType Type { get; private set; }
        public string? CustomTypeDescription { get; private set; }
        public ReporterInfo Reporter { get; private set; }


        private readonly List<DisasterVolunteer> _volunteers = new();
        public IReadOnlyList<DisasterVolunteer> Volunteers => _volunteers.AsReadOnly();


        private readonly List<DisasterResource> _resources = new();
        public IReadOnlyList<DisasterResource> Resources => _resources.AsReadOnly();


        private readonly List<AffectedPerson> _affectedPeople = new();
        public IReadOnlyList<AffectedPerson> AffectedPeople => _affectedPeople.AsReadOnly();

        public Report? Report { get; private set; }

        private Disaster()
        {

        }


        private Disaster(
            Guid id,
            DisasterType type,
            string title,
            string description,
            GeoLocation location,
            DateTimeOffset startTime,
            ReporterInfo reporter,
            string? customeTypeDescription)
            : base(id)
        {
            Title = title;
            Description = description;
            Location = location;
            StartTime = startTime;
            Reporter = reporter;
            Type = type;
            Status = DisasterStatus.Reported;
            CustomTypeDescription = customeTypeDescription;

        }

        public static ErrorOr<Disaster> Create(
            Guid id,
            DisasterType type,
            string title,
            string description,
            GeoLocation location,
            DateTimeOffset startTime,
            ReporterInfo reporter,
            string? customeTypeDescription)
        {
            if (id == Guid.Empty)
                return DomainErrors.IdMustBeProvided(nameof(Disaster));

            if (string.IsNullOrWhiteSpace(title))
                return DisasterErrors.TitleRequired;

            if (string.IsNullOrWhiteSpace(description))
                return DisasterErrors.DescriptionRequired;

            if (location is null)
                return DisasterErrors.LocationRequired;

            if (reporter is null)
                return DisasterErrors.ReporterInfoRequired;

            if (type == DisasterType.Other && string.IsNullOrWhiteSpace(customeTypeDescription))
                return DisasterErrors.CustomTypeDescriptionRequired;

            return new Disaster(
                id,
                type,
                title,
                description,
                location,
                startTime,
                reporter,
                customeTypeDescription);
        }

        public ErrorOr<Updated> StartResponse()
        {
            if (Status != DisasterStatus.Reported)
                return DisasterErrors.InvalidStatusTransition(Status, DisasterStatus.InProgress);

            Status = DisasterStatus.InProgress;

            return Result.Updated;
        }

        public ErrorOr<Updated> Resolve(DateTimeOffset date)
        {
            if (Status != DisasterStatus.InProgress)
                return DisasterErrors.InvalidStatusTransition(Status, DisasterStatus.Resolved);

            Status = DisasterStatus.Resolved;
            EndTime = date;

            return Result.Updated;
        }

        public ErrorOr<Updated> Close()
        {
            if (Status != DisasterStatus.Resolved)
                return DisasterErrors.InvalidStatusTransition(Status, DisasterStatus.Closed);

            Status = DisasterStatus.Closed;
            return Result.Updated;
        }

        public ErrorOr<Updated> Cancel(DateTimeOffset date)
        {
            if (Status == DisasterStatus.Resolved || Status == DisasterStatus.Closed || Status == DisasterStatus.Cancelled)
                return DisasterErrors.InvalidStatusTransition(Status, DisasterStatus.Cancelled);

            Status = DisasterStatus.Cancelled;
            EndTime = date;
            return Result.Updated;
        }

        public ErrorOr<Updated> AssignVolunteer(Guid volunteerId)
        {
            if (Status != DisasterStatus.InProgress)
                return DisasterErrors.CannotAssignVolunteerWhenNotInProgress;

            if (_volunteers.Any(v => v.VolunteerId == volunteerId))
                return DisasterErrors.VolunteerAlreadyAssigned;


            var volunteer = DisasterVolunteer.Create(Guid.NewGuid(), volunteerId, Id);

            if (volunteer.IsError)
                return volunteer.Errors;

            _volunteers.Add(volunteer.Value);

            return Result.Updated;


        }

        public ErrorOr<Updated> AssignVolunteers(List<Guid> volunteerIds)
        {
            if (Status != DisasterStatus.InProgress)
                return DisasterErrors.CannotAssignVolunteerWhenNotInProgress;

            foreach (var volunteerId in volunteerIds)
            {
                if (_volunteers.Any(v => v.VolunteerId == volunteerId))
                    continue;

                var volunteerResult = DisasterVolunteer.Create(Guid.NewGuid(), volunteerId, Id);

                if (volunteerResult.IsError)
                    return volunteerResult.Errors;

                _volunteers.Add(volunteerResult.Value);
            }
            return Result.Updated;
        }

        public ErrorOr<Updated> EvaluateVolunteer(
           Guid volunteerId,
           EvaluationScores evaluation,
           string? notes,
           DateTimeOffset evaluatedAt,
           Guid evaluatedByLeaderId)
        {

            var volunteer = _volunteers.FirstOrDefault(v => v.VolunteerId == volunteerId);

            if (volunteer is null)
                return DisasterErrors.volunteerNotFound;

            return volunteer.Evaluate(evaluation, notes, evaluatedByLeaderId, evaluatedAt);



        }

        public ErrorOr<Updated> RemoveVolunteer(Guid volunteerId)
        {
            var volunteer = _volunteers.FirstOrDefault(v => v.VolunteerId == volunteerId);
            if (volunteer is null)
                return DisasterErrors.volunteerNotFound;

            if (Status != DisasterStatus.Reported)
                return DisasterErrors.CannotRemoveVolunteerWhenNotInReportedStatus;

            _volunteers.Remove(volunteer);

            return Result.Updated;


        }


        public ErrorOr<Updated> AssignResource(Guid resourceId, int quantitySent, DateTimeOffset assignedAt, string? notes = null)
        {
            if (resourceId == Guid.Empty)
                return DomainErrors.IdMustBeProvided("Resource");

            if (Status != DisasterStatus.InProgress)
                return DisasterErrors.CannotAssignResourceWhenNotInProgress;

            var resource = _resources.FirstOrDefault(r => r.ResourceId == resourceId);



            if (resource is not null)
            {
                resource.IncreaseQuantity(quantitySent);

                return Result.Updated;
            }

            if (quantitySent <= 0)
                return DisasterErrors.ResourceQuantityshouldBeGreaterThanZero;

            var res = DisasterResource.Create(Guid.NewGuid(), Id, resourceId, quantitySent, assignedAt, notes);

            if (res.IsError)
                return res.Errors;

            _resources.Add(res.Value);

            return Result.Updated;

        }


        public ErrorOr<Updated> AddAffectedPersons(
            IEnumerable<
                (
                string name,
                int age,
                string phone,
                HealthStatus status,
                string? notes)>
            affectedPersons)
        {

            if (affectedPersons is null || !affectedPersons.Any())
                return Error.Validation(code: "Disaster.AffectedPersons.DataRequired", description: "affected persons data must be provided");

            if (Status != DisasterStatus.Resolved)
                return DisasterErrors.CannotAddAffectedPersonsWhenDisasterNotResolved;

            foreach (var person in affectedPersons)
            {

                var personToAdd = AffectedPerson.Create(Guid.NewGuid(), Id, person.name, person.age, person.phone, person.status, person.notes);

                if (personToAdd.IsError)
                    return personToAdd.Errors;

                _affectedPeople.Add(personToAdd.Value);
            }
            return Result.Updated;
        }

        public ErrorOr<Updated> UpdateAffectedPerson(Guid id,
                string name,
                int age,
                string phone,
                HealthStatus status,
                string? notes)
        {
            var person = _affectedPeople.FirstOrDefault(p => p.Id == id);

            if (person is null)
                return DisasterErrors.AffectedPeronNotFound;

            if (Status != DisasterStatus.Resolved)
                return DisasterErrors.CannotAddAffectedPersonsWhenDisasterNotResolved;

            return person.Update(name, age, phone, status, notes);
        }

        public ErrorOr<Updated> RemoveAffectedPerson(Guid id)
        {
            var person = _affectedPeople.FirstOrDefault(p => p.Id == id);

            if (person is null)
                return DisasterErrors.AffectedPeronNotFound;

            if (Status != DisasterStatus.Resolved)
                return DisasterErrors.CannotAddAffectedPersonsWhenDisasterNotResolved;

            _affectedPeople.Remove(person);
            return Result.Updated;

        }

        public ErrorOr<Updated> AddReport(string summary, string teams, string resources, string affectedPersons, DateTimeOffset issuedAt)
        {

            if (Report is not null)
                return DisasterErrors.ReportAlreadyExists;

            if (Status != DisasterStatus.Closed)
                return DisasterErrors.CannotGenerateReportWhenDisasterNotClosed;

             var report = Report.Create( summary, teams, resources, affectedPersons, issuedAt);
            
            if (report.IsError)
                return report.Errors;

            return Result.Updated;
        }
    }
}
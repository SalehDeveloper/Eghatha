using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Disaster;
using Eghatha.Domain.Disasters.AffectedPersons;
using Eghatha.Domain.Disasters.DisasterResources;
using Eghatha.Domain.Disasters.DisasterVolunteers;
using Eghatha.Domain.Disasters.Events;
using Eghatha.Domain.Disasters.Reports;
using Eghatha.Domain.Shared.Errors;
using Eghatha.Domain.Shared.ValueObjects;
using Eghatha.Domain.Teams.Resources;
using ErrorOr;
using System.Security.AccessControl;


namespace Eghatha.Domain.Disasters
{
    public sealed class Disaster : AuditableEntity
    {
        public string Title { get; private set; }

        public string Description { get; private set; }

        public GeoLocation Location { get; private set; }

        public string Province { get; private set; }

        public string City { get; private set; }

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

        private readonly List<DisasterTeam> _teams = new();
        public IReadOnlyList<DisasterTeam> Teams => _teams.AsReadOnly();

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
            string province,
            string city,
            DateTimeOffset startTime,
            ReporterInfo reporter,
            string? customeTypeDescription)
            : base(id)
        {
            Title = title;
            Description = description;
            Location = location;
            Province = province;
            City = city;
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
            string province,
            string city,
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

            if (string.IsNullOrWhiteSpace(province))
                return DisasterErrors.ProvinceRequired;

            if (string.IsNullOrWhiteSpace(city))
                return DisasterErrors.CityRequired;


            if (location is null)
                return DisasterErrors.LocationRequired;

            if (reporter is null)
                return DisasterErrors.ReporterInfoRequired;

            if (type == DisasterType.Other && string.IsNullOrWhiteSpace(customeTypeDescription))
                return DisasterErrors.CustomTypeDescriptionRequired;


            var disaster = new Disaster(
                id,
                type,
                title,
                description,
                location,
                province,
                city,
                startTime,
                reporter,
                customeTypeDescription);

            disaster.AddDomainEvent(new DisasterCreated(id, location.Latitude, location.Longitude, province, city, type, customeTypeDescription, startTime));

            return disaster;
        }

        public ErrorOr<Updated> StartResponse()
        {
            if (Status != DisasterStatus.Reported)
                return DisasterErrors.InvalidStatusTransition(Status, DisasterStatus.InProgress);

            Status = DisasterStatus.InProgress;

            AddDomainEvent(new DisasterResponseStarted(Id, Status, StartTime));
            return Result.Updated;
        }

        public ErrorOr<Updated> Resolve(DateTimeOffset date)
        {
            if (Status != DisasterStatus.InProgress)
                return DisasterErrors.InvalidStatusTransition(Status, DisasterStatus.Resolved);

            Status = DisasterStatus.Resolved;
            EndTime = date;

            AddDomainEvent(new DisasterResolved(Id, Status, date));

            return Result.Updated;
        }

        public ErrorOr<Updated> Close()
        {
            if (Status != DisasterStatus.Resolved)
                return DisasterErrors.InvalidStatusTransition(Status, DisasterStatus.Closed);

            // check if there are any volunteer without evaluation 
            if (_volunteers.Any(v => v.EvaluationScores==null))
                return DisasterErrors.CannotCloseDisasterWithUnevaluatedVolunteers;


            // check of there are any resource not managed 
            if (_resources.Any(r => r.QuantitySent != r.QuantityReturned + r.QuantityConsumed + r.QuantityDamaged))
                return DisasterErrors.CannotCloseDisasterWithUnmanagedResources;

            Status = DisasterStatus.Closed;

            AddDomainEvent(new DisasterClosed(Id, Status));
            return Result.Updated;
        }

        public ErrorOr<Updated> Archive()
        {
            if (Status != DisasterStatus.Closed)
                return DisasterErrors.InvalidStatusTransition(Status, DisasterStatus.Archived);

            if (Report is null)
                return DisasterErrors.CannotArchiveWithoutReport;
            Status = DisasterStatus.Archived;

            AddDomainEvent(new DisasterArchived(Id, Status));
            return Result.Updated;
        }

        public ErrorOr<Updated> Cancel(DateTimeOffset date)
        {
            if (Status == DisasterStatus.Resolved || Status == DisasterStatus.Closed || Status == DisasterStatus.Cancelled)
                return DisasterErrors.InvalidStatusTransition(Status, DisasterStatus.Cancelled);

            Status = DisasterStatus.Cancelled;
            EndTime = date;

            AddDomainEvent(new DisasterCancelled(Id, Status, date));
            return Result.Updated;
        }

        public ErrorOr<List<DisasterVolunteer>> AssignVolunteers(IEnumerable<Guid> volunteerIds)
        {
            if (Status != DisasterStatus.Reported &&
                Status != DisasterStatus.InProgress)
            {
                return DisasterErrors.CannotAssignVolunteerWhenNotInValidStatus;
            }

            var newVolunteers = new List<DisasterVolunteer>();

            foreach (var volunteerId in volunteerIds.Distinct())
            {
                if (_volunteers.Any(v => v.VolunteerId == volunteerId))
                    continue;

                var volunteer = DisasterVolunteer.Create(
                    Guid.NewGuid(),
                    volunteerId,
                    Id);

                if (volunteer.IsError)
                    return volunteer.Errors;

                newVolunteers.Add(volunteer.Value);
            }

            if (newVolunteers.Count > 0)
            {
                AddDomainEvent(new VolunteersAssignedToDisaster(
                    Id,
                    newVolunteers.Select(v => v.VolunteerId).ToList(),
                    Location,
                    City,
                    Province,
                    Type.Name,
                    StartTime,
                    Title,
                    Description
                ));

                
            }
          

            return newVolunteers;
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

            var res =  volunteer.Evaluate(evaluation, notes, evaluatedByLeaderId, evaluatedAt);

            if (res.IsError) return res.Errors;

            AddDomainEvent(new VolunteerEvaluated(volunteer.VolunteerId, Id, evaluation.TotalScore, evaluatedAt));

            return Result.Updated;

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


        public ErrorOr<Updated> AssignTeam(Guid teamId)
        {
            if (teamId == Guid.Empty)
                return DomainErrors.IdMustBeProvided("Team");

            if (Status != DisasterStatus.Reported && Status != DisasterStatus.InProgress)
                return DisasterErrors.CannotAssignTeamWhenNotInValidStatus;

            if (Teams.Any(t => t.TeamId == teamId))
                return DisasterErrors.TeamAlreadyAssigned;

            _teams.Add(new DisasterTeam(Id, teamId));

            AddDomainEvent(new TeamAssignedToDisasterEvent(Id, teamId, Title, City));
            return Result.Updated;
        }

        public ErrorOr<Updated> RemoveTeam(Guid teamId)
        {
            var team = _teams.FirstOrDefault(t => t.TeamId == teamId);
            if (team is null)
                return DisasterErrors.TeamNotFound;


            if (Status != DisasterStatus.Reported)
                return DisasterErrors.CannotRemoveVolunteerWhenNotInReportedStatus;

            _teams.Remove(team);

            return Result.Updated;


        }


        public ErrorOr<DispatchResourceResult> DispatchResource(Guid resourceId, Guid teamId, Teams.Resources.ResourceType resourceType, int quantitySent, DateTimeOffset assignedAt, string? notes = null)
        {
            if (resourceId == Guid.Empty)
                return DomainErrors.IdMustBeProvided("Resource");

            if (teamId == Guid.Empty)
                return DomainErrors.IdMustBeProvided("Team");

            if (!_teams.Any(t => t.TeamId == teamId))
                return DisasterErrors.TeamNotAssignedToDisaster;

            if (Status != DisasterStatus.Reported && Status != DisasterStatus.InProgress)
                return DisasterErrors.CannotAssignVolunteerWhenNotInValidStatus;

            var resource = _resources.FirstOrDefault(r => r.ResourceId == resourceId);



            if (resource is not null)
            {
                resource.IncreaseQuantity(quantitySent);

                return new DispatchResourceResult(resource , false);
            }

            if (quantitySent <= 0)
                return DisasterErrors.ResourceQuantityshouldBeGreaterThanZero;

            var res = DisasterResource.Create(Guid.NewGuid(), Id, resourceId, resourceType, teamId, quantitySent, assignedAt, notes);

            if (res.IsError)
                return res.Errors;

            _resources.Add(res.Value);

            AddDomainEvent(new ResourceDispatchedToDisaster(Id , resourceId, quantitySent , teamId , resourceType));

            return   new DispatchResourceResult(res.Value, true); ;

        }

        public ErrorOr<Updated> ConsumeResource(Guid resourceId, int quantity)
        {
            if (resourceId == Guid.Empty)
                return DomainErrors.IdMustBeProvided("Resource");

            if (quantity <= 0)
                return DisasterErrors.ResourceQuantityshouldBeGreaterThanZero;

            var resource = _resources.FirstOrDefault(r => r.Id == resourceId);

            if (resource is null)
                return DisasterResourceErrors.ResourceNotFound;

            if (!resource.ResourceType.IsConsumable)
                return DisasterResourceErrors.ResourceIsNotConsumable;


            
            var res =  resource.Consume(quantity);

            if (res.IsError)
                return res.Errors;

            AddDomainEvent(new ResourceConsumed(Id, resource.ResourceId , quantity , resource.TeamId , resource.ResourceType));

            return res;
        }

        public ErrorOr<Updated> ReturnResource(Guid disasterResourceId, int quantity)
        {
            if (disasterResourceId == Guid.Empty)
                return DomainErrors.IdMustBeProvided("DisasterResource");

            if (quantity <= 0)
                return DisasterErrors.ResourceQuantityshouldBeGreaterThanZero;

            var resource = _resources
                .FirstOrDefault(r => r.Id == disasterResourceId);

            if (resource is null)
                return DisasterResourceErrors.ResourceNotFound;

            var res =  resource.Return(quantity);

            if (res.IsError)
                return res.Errors;

            AddDomainEvent(new ResourceReturned(Id, resource.ResourceId, resource.TeamId, quantity , resource.ResourceType));

            return res;
        }

        public ErrorOr<Updated> MarkResourceAsDamaged(Guid disasterResourceId, int quantity)
        {
            if (disasterResourceId == Guid.Empty)
                return DomainErrors.IdMustBeProvided("DisasterResource");

            if (quantity <= 0)
                return DisasterErrors.ResourceQuantityshouldBeGreaterThanZero;

            var resource = _resources
                .FirstOrDefault(r => r.Id == disasterResourceId);

            if (resource is null)
                return DisasterResourceErrors.ResourceNotFound;

            var res=  resource.MarkDamaged(quantity);
            if (res.IsError) return res.Errors;

            AddDomainEvent(new ResourceDamaged(Id , resource.ResourceId , resource.TeamId , quantity , resource.ResourceType));

            return res;

        }

        public ErrorOr<List<AffectedPerson>> AddAffectedPersons(
            IEnumerable<
                (
                string name,
                int age,
                string phone,
                HealthStatus status,
                string? notes)>
            affectedPersons)
        {
            var affected = new List<AffectedPerson>();
            
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
                affected.Add(personToAdd.Value);
            }

            AddDomainEvent(new AffectedPersonsAdded(Id, affected.Select(p => p.Id).ToList()));
            return affected;
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

        public ErrorOr<Report> AddReport(Report report )
        {
            if (report is null)
                return Error.Validation(
                    code: "Report.Required",
                    description: "Report is required.");

            if (Status != DisasterStatus.Closed)
                return DisasterErrors.CannotGenerateReportWhenDisasterNotClosed;

            if (Report is not null)
                return DisasterErrors.ReportAlreadyExists;

            Report = report;

            AddDomainEvent(new DisasterReportGenerated(Id , Report.Id));
            return Report;

        }
    }
}
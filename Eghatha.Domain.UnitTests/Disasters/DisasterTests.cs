using Eghatha.Domain.Disaster;
using Eghatha.Domain.Disasters;
using Eghatha.Domain.Disasters.AffectedPersons;
using Eghatha.Domain.Disasters.DisasterResources;
using Eghatha.Domain.Disasters.DisasterVolunteers;
using Eghatha.Domain.Disasters.Events;
using Eghatha.Domain.Shared.Errors;
using Eghatha.Domain.Shared.ValueObjects;
using Eghatha.Domain.Teams.Resources;
using Eghatha.Tests.Common.Disasters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.UnitTests.Disasters
{
    public class DisasterTests
    {
        public static IEnumerable<object[]> NonReportedDisasters()
        {
            yield return new object[] { DisasterTestFactory.CreateInProgress() };
            yield return new object[] { DisasterTestFactory.CreateResolved() };
            yield return new object[] { DisasterTestFactory.CreateClosed() };
            yield return new object[] { DisasterTestFactory.CreateArchived() };
            yield return new object[] { DisasterTestFactory.CreateCancelled() };
        }

        public static IEnumerable<object[]> NonInProgressDisasters()
        {
            yield return new object[] { DisasterTestFactory.CreateReported() };
            yield return new object[] { DisasterTestFactory.CreateResolved() };
            yield return new object[] { DisasterTestFactory.CreateClosed() };
            yield return new object[] { DisasterTestFactory.CreateArchived() };
            yield return new object[] { DisasterTestFactory.CreateCancelled() };
        }

        public static IEnumerable<object[]> NonResolvedDisasters()
        {
            yield return new object[] { DisasterTestFactory.CreateReported() };
            yield return new object[] { DisasterTestFactory.CreateInProgress() };
            yield return new object[] { DisasterTestFactory.CreateClosed() };
            yield return new object[] { DisasterTestFactory.CreateArchived() };
            yield return new object[] { DisasterTestFactory.CreateCancelled() };
        }

        public static IEnumerable<object[]> NonClosedDisasters()
        {
            yield return new object[] { DisasterTestFactory.CreateReported() };
            yield return new object[] { DisasterTestFactory.CreateInProgress() };
            yield return new object[] { DisasterTestFactory.CreateResolved() };
            yield return new object[] { DisasterTestFactory.CreateArchived() };
            yield return new object[] { DisasterTestFactory.CreateCancelled() };
        }

        public static IEnumerable<object[]> InvalidStatusesForAssignment()
        {
            yield return new object[] { DisasterTestFactory.CreateResolved() };
            yield return new object[] { DisasterTestFactory.CreateClosed() };
            yield return new object[] { DisasterTestFactory.CreateArchived() };
            yield return new object[] { DisasterTestFactory.CreateCancelled() };
        }

      
        private static (string name, int age, string phone, HealthStatus status, string? notes) ValidPerson(
            string name = "Sara Ali") => (name, 34, "0999888777", HealthStatus.Injured, "stable");


        [Fact]
        public void Create_WithValidData_ReturnsDisasterInReportedStatus()
        {
            var reporter = ReporterInfo.Create("Ahmad", "0102030405", "0999111222").Value;
            var location = GeoLocation.Create(36.2021, 37.1343).Value;
            var startTime = new DateTimeOffset(2026, 1, 1, 8, 0, 0, TimeSpan.Zero);

            var result = DisasterBuilder.Valid()
                .WithType(DisasterType.Earthquake)
                .WithTitle("Al-Bab Earthquake")
                .WithDescription("6.2 magnitude earthquake")
                .WithLocation(location)
                .WithProvince("Aleppo")
                .WithCity("Al-Bab")
                .WithStartTime(startTime)
                .WithReporter(reporter)
                .Build();

            Assert.False(result.IsError);
            var disaster = result.Value;
            Assert.Equal(DisasterType.Earthquake, disaster.Type);
            Assert.Equal("Al-Bab Earthquake", disaster.Title);
            Assert.Equal("6.2 magnitude earthquake", disaster.Description);
            Assert.Equal(location, disaster.Location);
            Assert.Equal("Aleppo", disaster.Province);
            Assert.Equal("Al-Bab", disaster.City);
            Assert.Equal(startTime, disaster.StartTime);
            Assert.Equal(reporter, disaster.Reporter);
            Assert.Equal(DisasterStatus.Reported, disaster.Status);
            Assert.Null(disaster.EndTime);
            Assert.Null(disaster.CustomTypeDescription);
            Assert.Empty(disaster.Volunteers);
            Assert.Empty(disaster.Resources);
            Assert.Empty(disaster.AffectedPeople);
            Assert.Empty(disaster.Teams);
            Assert.Null(disaster.Report);
        }

        [Fact]
        public void Create_WithValidData_RaisesDisasterCreatedEventWithCorrectPayload()
        {
            var location = GeoLocation.Create(36.2, 37.1).Value;
            var startTime = new DateTimeOffset(2026, 1, 1, 8, 0, 0, TimeSpan.Zero);

            var disaster = DisasterBuilder.Valid()
                .WithType(DisasterType.Fire)
                .WithLocation(location)
                .WithProvince("Aleppo")
                .WithCity("Al-Bab")
                .WithStartTime(startTime)
                .BuildValid();

            var raised = Assert.Single(disaster.DomainEvents.OfType<DisasterCreated>());
            Assert.Equal(disaster.Id, raised.Id);
            Assert.Equal(location.Latitude, raised.Latitude);
            Assert.Equal(location.Longitude, raised.Longitude);
            Assert.Equal("Aleppo", raised.Province);
            Assert.Equal("Al-Bab", raised.City);
            Assert.Equal(DisasterType.Fire, raised.Type);
            Assert.Equal(startTime, raised.OccuredAt);
        }

        [Fact]
        public void Create_WithEmptyId_ReturnsIdMustBeProvidedError()
        {
            var result = DisasterBuilder.Valid().WithId(Guid.Empty).Build();

            Assert.True(result.IsError);
            Assert.Equal(DomainErrors.IdMustBeProvided(nameof(Disaster)), result.FirstError);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithMissingTitle_ReturnsTitleRequiredError(string? title)
        {
            var result = DisasterBuilder.Valid().WithTitle(title!).Build();

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.TitleRequired, result.FirstError);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithMissingDescription_ReturnsDescriptionRequiredError(string? description)
        {
            var result = DisasterBuilder.Valid().WithDescription(description!).Build();

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.DescriptionRequired, result.FirstError);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithMissingProvince_ReturnsProvinceRequiredError(string? province)
        {
            var result = DisasterBuilder.Valid().WithProvince(province!).Build();

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.ProvinceRequired, result.FirstError);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithMissingCity_ReturnsCityRequiredError(string? city)
        {
            var result = DisasterBuilder.Valid().WithCity(city!).Build();

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.CityRequired, result.FirstError);
        }

        [Fact]
        public void Create_WithNullLocation_ReturnsLocationRequiredError()
        {
            var result = DisasterBuilder.Valid().WithLocation(null!).Build();

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.LocationRequired, result.FirstError);
        }

        [Fact]
        public void Create_WithNullReporter_ReturnsReporterInfoRequiredError()
        {
            var result = DisasterBuilder.Valid().WithReporter(null!).Build();

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.ReporterInfoRequired, result.FirstError);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithOtherTypeAndMissingCustomDescription_ReturnsCustomTypeDescriptionRequiredError(string? customDescription)
        {
            var result = DisasterBuilder.Valid()
                .WithType(DisasterType.Other)
                .WithCustomTypeDescription(customDescription)
                .Build();

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.CustomTypeDescriptionRequired, result.FirstError);
        }

        [Fact]
        public void Create_WithOtherTypeAndCustomDescription_Succeeds()
        {
            var result = DisasterBuilder.Valid()
                .WithType(DisasterType.Other)
                .WithCustomTypeDescription("Unclassified structural collapse")
                .Build();

            Assert.False(result.IsError);
            Assert.Equal("Unclassified structural collapse", result.Value.CustomTypeDescription);
        }

        [Fact]
        public void Create_WithNonOtherTypeAndNoCustomDescription_Succeeds()
        {
            var result = DisasterBuilder.Valid()
                .WithType(DisasterType.Flood)
                .WithCustomTypeDescription(null)
                .Build();

            Assert.False(result.IsError);
        }

        [Fact]
        public void StartResponse_WhenReported_TransitionsToInProgressAndRaisesEvent()
        {
            var disaster = DisasterTestFactory.CreateReported();

            var result = disaster.StartResponse();

            Assert.False(result.IsError);
            Assert.Equal(DisasterStatus.InProgress, disaster.Status);
            var raised = Assert.Single(disaster.DomainEvents.OfType<DisasterResponseStarted>());
            Assert.Equal(disaster.Id, raised.Id);
            Assert.Equal(DisasterStatus.InProgress, raised.Status);
        }

        [Theory]
        [MemberData(nameof(NonReportedDisasters))]
        public void StartResponse_WhenNotReported_ReturnsInvalidStatusTransitionError(Domain.Disasters.Disaster disaster)
        {
            var startingStatus = disaster.Status;

            var result = disaster.StartResponse();

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.InvalidStatusTransition(startingStatus, DisasterStatus.InProgress), result.FirstError);
            Assert.Equal(startingStatus, disaster.Status);
        }

        // ---------- Resolve ----------

        [Fact]
        public void Resolve_WhenInProgress_TransitionsToResolvedSetsEndTimeAndRaisesEvent()
        {
            var disaster = DisasterTestFactory.CreateInProgress();
            var resolvedAt = new DateTimeOffset(2026, 3, 1, 10, 0, 0, TimeSpan.Zero);

            var result = disaster.Resolve(resolvedAt);

            Assert.False(result.IsError);
            Assert.Equal(DisasterStatus.Resolved, disaster.Status);
            Assert.Equal(resolvedAt, disaster.EndTime);
            var raised = Assert.Single(disaster.DomainEvents.OfType<DisasterResolved>());
            Assert.Equal(disaster.Id, raised.Id);
            Assert.Equal(DisasterStatus.Resolved, raised.Status);
            Assert.Equal(resolvedAt, raised.ResolvedAt);
        }

        [Theory]
        [MemberData(nameof(NonInProgressDisasters))]
        public void Resolve_WhenNotInProgress_ReturnsInvalidStatusTransitionError(Domain.Disasters.Disaster disaster)
        {
            var startingStatus = disaster.Status;

            var result = disaster.Resolve(DateTimeOffset.UtcNow);

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.InvalidStatusTransition(startingStatus, DisasterStatus.Resolved), result.FirstError);
        }

        // ---------- Close ----------

        [Fact]
        public void Close_WhenResolvedWithNoVolunteersOrResources_TransitionsToClosedAndRaisesEvent()
        {
            var disaster = DisasterTestFactory.CreateResolved();

            var result = disaster.Close();

            Assert.False(result.IsError);
            Assert.Equal(DisasterStatus.Closed, disaster.Status);
            var raised = Assert.Single(disaster.DomainEvents.OfType<DisasterClosed>());
            Assert.Equal(disaster.Id, raised.Id);
            Assert.Equal(DisasterStatus.Closed, raised.Status);
        }

        [Theory]
        [MemberData(nameof(NonResolvedDisasters))]
        public void Close_WhenNotResolved_ReturnsInvalidStatusTransitionError(Domain.Disasters.Disaster disaster)
        {
            var startingStatus = disaster.Status;

            var result = disaster.Close();

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.InvalidStatusTransition(startingStatus, DisasterStatus.Closed), result.FirstError);
        }

        [Fact]
        public void Close_WithUnevaluatedVolunteer_ReturnsCannotCloseDisasterWithUnevaluatedVolunteersError()
        {
            var disaster = DisasterTestFactory.CreateReported();
            var volunteerId = Guid.NewGuid();
            disaster.AssignVolunteers(new[] { volunteerId });
            disaster.StartResponse();
            disaster.Resolve(DateTimeOffset.UtcNow);

            var result = disaster.Close();

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.CannotCloseDisasterWithUnevaluatedVolunteers, result.FirstError);
            Assert.Equal(DisasterStatus.Resolved, disaster.Status);
        }

        [Fact]
        public void Close_WithAllVolunteersEvaluated_Succeeds()
        {
            var disaster = DisasterTestFactory.CreateReported();
            var volunteerId = Guid.NewGuid();
            disaster.AssignVolunteers(new[] { volunteerId });
            var evaluation = EvaluationScores.Create(5, 5, 5, 5, 5).Value;
            disaster.EvaluateVolunteer(volunteerId, evaluation, "great job", DateTimeOffset.UtcNow, Guid.NewGuid());
            disaster.StartResponse();
            disaster.Resolve(DateTimeOffset.UtcNow);

            var result = disaster.Close();

            Assert.False(result.IsError);
            Assert.Equal(DisasterStatus.Closed, disaster.Status);
        }

        [Fact]
        public void Close_WithUnmanagedResources_ReturnsCannotCloseDisasterWithUnmanagedResourcesError()
        {
            var disaster = DisasterTestFactory.CreateReported();
            var teamId = Guid.NewGuid();
            disaster.AssignTeam(teamId);
            disaster.DispatchResource(Guid.NewGuid(), teamId, ResourceType.FirstAidKit, 10, DateTimeOffset.UtcNow);
            disaster.StartResponse();
            disaster.Resolve(DateTimeOffset.UtcNow);

            var result = disaster.Close();

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.CannotCloseDisasterWithUnmanagedResources, result.FirstError);
        }

        [Fact]
        public void Close_WithFullyManagedResources_Succeeds()
        {
            var disaster = DisasterTestFactory.CreateReported();
            var teamId = Guid.NewGuid();
            disaster.AssignTeam(teamId);
            var dispatchResult = disaster.DispatchResource(Guid.NewGuid(), teamId, ResourceType.FirstAidKit, 10, DateTimeOffset.UtcNow);
            var resourceId = dispatchResult.Value.Resource.Id;
            disaster.ConsumeResource(resourceId, 10);
            disaster.StartResponse();
            disaster.Resolve(DateTimeOffset.UtcNow);

            var result = disaster.Close();

            Assert.False(result.IsError);
            Assert.Equal(DisasterStatus.Closed, disaster.Status);
        }

        // ---------- Archive ----------

        [Fact]
        public void Archive_WhenClosedWithReport_TransitionsToArchivedAndRaisesEvent()
        {
            var disaster = DisasterTestFactory.CreateClosedWithReport(out _);

            var result = disaster.Archive();

            Assert.False(result.IsError);
            Assert.Equal(DisasterStatus.Archived, disaster.Status);
            var raised = Assert.Single(disaster.DomainEvents.OfType<DisasterArchived>());
            Assert.Equal(disaster.Id, raised.Id);
            Assert.Equal(DisasterStatus.Archived, raised.Status);
        }

        [Theory]
        [MemberData(nameof(NonClosedDisasters))]
        public void Archive_WhenNotClosed_ReturnsInvalidStatusTransitionError(Domain.Disasters.Disaster disaster)
        {
            var startingStatus = disaster.Status;

            var result = disaster.Archive();

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.InvalidStatusTransition(startingStatus, DisasterStatus.Archived), result.FirstError);
        }

        [Fact]
        public void Archive_WhenClosedWithoutReport_ReturnsCannotArchiveWithoutReportError()
        {
            var disaster = DisasterTestFactory.CreateClosed();

            var result = disaster.Archive();

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.CannotArchiveWithoutReport, result.FirstError);
            Assert.Equal(DisasterStatus.Closed, disaster.Status);
        }

        // ---------- Cancel ----------

        [Fact]
        public void Cancel_WhenReported_TransitionsToCancelledSetsEndTimeAndRaisesEvent()
        {
            var disaster = DisasterTestFactory.CreateReported();
            var cancelledAt = new DateTimeOffset(2026, 1, 10, 9, 0, 0, TimeSpan.Zero);

            var result = disaster.Cancel(cancelledAt);

            Assert.False(result.IsError);
            Assert.Equal(DisasterStatus.Cancelled, disaster.Status);
            Assert.Equal(cancelledAt, disaster.EndTime);
            var raised = Assert.Single(disaster.DomainEvents.OfType<DisasterCancelled>());
            Assert.Equal(disaster.Id, raised.Id);
            Assert.Equal(DisasterStatus.Cancelled, raised.Status);
            Assert.Equal(cancelledAt, raised.CancelledAt);
        }

        [Theory]
        [MemberData(nameof(NonReportedDisasters))]
        public void Cancel_WhenNotReported_ReturnsInvalidStatusTransitionError(Domain.Disasters.Disaster disaster)
        {
            var startingStatus = disaster.Status;

            var result = disaster.Cancel(DateTimeOffset.UtcNow);

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.InvalidStatusTransition(startingStatus, DisasterStatus.Cancelled), result.FirstError);
        }


        [Fact]
        public void AssignVolunteers_WhenReported_AddsVolunteersAndRaisesEvent()
        {
            var disaster = DisasterTestFactory.CreateReported();
            var volunteerIds = new[] { Guid.NewGuid(), Guid.NewGuid() };

            var result = disaster.AssignVolunteers(volunteerIds);

            Assert.False(result.IsError);
            Assert.Equal(2, result.Value.Count);
            Assert.Equal(2, disaster.Volunteers.Count);
            Assert.All(volunteerIds, id => Assert.Contains(disaster.Volunteers, v => v.VolunteerId == id));

            var raised = Assert.Single(disaster.DomainEvents.OfType<VolunteersAssignedToDisaster>());
            Assert.Equal(disaster.Id, raised.DisasterId);
            Assert.Equal(volunteerIds.OrderBy(x => x), raised.VolunteerIds.OrderBy(x => x));
        }

        [Fact]
        public void AssignVolunteers_WhenInProgress_AddsVolunteers()
        {
            var disaster = DisasterTestFactory.CreateInProgress();

            var result = disaster.AssignVolunteers(new[] { Guid.NewGuid() });

            Assert.False(result.IsError);
            Assert.Single(disaster.Volunteers);
        }

        [Theory]
        [MemberData(nameof(InvalidStatusesForAssignment))]
        public void AssignVolunteers_WhenNotReportedOrInProgress_ReturnsError(Domain.Disasters.Disaster disaster)
        {
            var result = disaster.AssignVolunteers(new[] { Guid.NewGuid() });

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.CannotAssignVolunteerWhenNotInValidStatus, result.FirstError);
            Assert.Empty(disaster.Volunteers);
        }

        [Fact]
        public void AssignVolunteers_WithDuplicateIdsInInput_OnlyAddsDistinctVolunteers()
        {
            var disaster = DisasterTestFactory.CreateReported();
            var volunteerId = Guid.NewGuid();

            var result = disaster.AssignVolunteers(new[] { volunteerId, volunteerId });

            Assert.False(result.IsError);
            Assert.Single(result.Value);
         
        }

        [Fact]
        public void AssignVolunteers_WithAlreadyAssignedVolunteer_SkipsExistingAndAddsOnlyNewOnes()
        {
            var disaster = DisasterTestFactory.CreateReported();
           
            var existingVolunteerId = Guid.NewGuid();
            var newVolunteerId = Guid.NewGuid();
           
            var assignedVol =  disaster.AssignVolunteers(new[] { existingVolunteerId });
            disaster.ClearDomainEvents();

            var result = disaster.AssignVolunteers(new[] { existingVolunteerId, newVolunteerId });

            Assert.False(result.IsError);
            var added = Assert.Single(result.Value);
            Assert.Equal(newVolunteerId, added.VolunteerId);
            Assert.Equal(2, disaster.Volunteers.Count);
        }

        [Fact]
        public void AssignVolunteers_WhenAllRequestedAlreadyAssigned_DoesNotRaiseANewEvent()
        {
            var disaster = DisasterTestFactory.CreateReported();
            var volunteerId = Guid.NewGuid();
            disaster.AssignVolunteers(new[] { volunteerId });
            disaster.ClearDomainEvents();

            var result = disaster.AssignVolunteers(new[] { volunteerId });

            Assert.False(result.IsError);
            Assert.Empty(result.Value);
            Assert.Empty(disaster.DomainEvents.OfType<VolunteersAssignedToDisaster>());
        }

        // ---------- EvaluateVolunteer ----------

        [Fact]
        public void EvaluateVolunteer_WhenAssigned_SetsScoresAndRaisesEvent()
        {
            var disaster = DisasterTestFactory.CreateReported();
            var volunteerId = Guid.NewGuid();
            var leaderId = Guid.NewGuid();
            disaster.AssignVolunteers(new[] { volunteerId });
            var evaluation = EvaluationScores.Create(4, 5, 3, 5, 4).Value;
            var evaluatedAt = DateTimeOffset.UtcNow;

            var result = disaster.EvaluateVolunteer(volunteerId, evaluation, "solid performance", evaluatedAt, leaderId);

            Assert.False(result.IsError);
            var volunteer = disaster.Volunteers.Single(v => v.VolunteerId == volunteerId);
            Assert.Equal(evaluation, volunteer.EvaluationScores);
            Assert.Equal("solid performance", volunteer.Notes);
            Assert.Equal(leaderId, volunteer.EvaluatedByLeaderId);
            Assert.Equal(evaluatedAt, volunteer.EvaluatedAt);

            var raised = Assert.Single(disaster.DomainEvents.OfType<VolunteerEvaluated>());
            Assert.Equal(volunteerId, raised.VolunteerId);
            Assert.Equal(disaster.Id, raised.DisasterId);
            Assert.Equal(evaluation.TotalScore, raised.TotalScore);
            Assert.Equal(evaluatedAt, raised.EvaluatedAt);
        }

        [Fact]
        public void EvaluateVolunteer_WhenNotAssigned_ReturnsVolunteerNotFoundError()
        {
            var disaster = DisasterTestFactory.CreateReported();
            var evaluation = EvaluationScores.Create(3, 3, 3, 3, 3).Value;

            var result = disaster.EvaluateVolunteer(Guid.NewGuid(), evaluation, null, DateTimeOffset.UtcNow, Guid.NewGuid());

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.volunteerNotFound, result.FirstError);
        }

        [Fact]
        public void EvaluateVolunteer_WithEmptyLeaderId_ReturnsIdMustBeProvidedError()
        {
            var disaster = DisasterTestFactory.CreateReported();
            var volunteerId = Guid.NewGuid();
            disaster.AssignVolunteers(new[] { volunteerId });
            var evaluation = EvaluationScores.Create(3, 3, 3, 3, 3).Value;

            var result = disaster.EvaluateVolunteer(volunteerId, evaluation, null, DateTimeOffset.UtcNow, Guid.Empty);

            Assert.True(result.IsError);
            Assert.Equal(DomainErrors.IdMustBeProvided("Leader"), result.FirstError);
        }

        // ---------- RemoveVolunteer ----------

        [Fact]
        public void RemoveVolunteer_WhenReportedAndAssigned_RemovesVolunteer()
        {
            var disaster = DisasterTestFactory.CreateReported();
            var volunteerId = Guid.NewGuid();
            disaster.AssignVolunteers(new[] { volunteerId });

            var result = disaster.RemoveVolunteer(volunteerId);

            Assert.False(result.IsError);
            Assert.Empty(disaster.Volunteers);
        }

        [Fact]
        public void RemoveVolunteer_WhenNotAssigned_ReturnsVolunteerNotFoundError()
        {
            var disaster = DisasterTestFactory.CreateReported();

            var result = disaster.RemoveVolunteer(Guid.NewGuid());

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.volunteerNotFound, result.FirstError);
        }

        [Fact]
        public void RemoveVolunteer_WhenNotReported_ReturnsCannotRemoveVolunteerWhenNotInReportedStatusError()
        {
            var disaster = DisasterTestFactory.CreateReported();
            var volunteerId = Guid.NewGuid();
            disaster.AssignVolunteers(new[] { volunteerId });
            disaster.StartResponse();

            var result = disaster.RemoveVolunteer(volunteerId);

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.CannotRemoveVolunteerWhenNotInReportedStatus, result.FirstError);
            Assert.Single(disaster.Volunteers);
        }


        // ---------- AssignTeam ----------

        [Fact]
        public void AssignTeam_WhenReported_AddsTeamAndRaisesEvent()
        {
            var disaster = DisasterTestFactory.CreateReported();
            var teamId = Guid.NewGuid();

            var result = disaster.AssignTeam(teamId);

            Assert.False(result.IsError);
            Assert.Contains(disaster.Teams, t => t.TeamId == teamId);

            var raised = Assert.Single(disaster.DomainEvents.OfType<TeamAssignedToDisasterEvent>());
            Assert.Equal(disaster.Id, raised.DisasterId);
            Assert.Equal(teamId, raised.TeamId);
            Assert.Equal(disaster.Title, raised.DisasterTitle);
            Assert.Equal(disaster.City, raised.City);
        }

        [Fact]
        public void AssignTeam_WhenInProgress_AddsTeam()
        {
            var disaster = DisasterTestFactory.CreateInProgress();

            var result = disaster.AssignTeam(Guid.NewGuid());

            Assert.False(result.IsError);
            Assert.Single(disaster.Teams);
        }

        [Fact]
        public void AssignTeam_WithEmptyTeamId_ReturnsIdMustBeProvidedError()
        {
            var disaster = DisasterTestFactory.CreateReported();

            var result = disaster.AssignTeam(Guid.Empty);

            Assert.True(result.IsError);
            Assert.Equal(DomainErrors.IdMustBeProvided("Team"), result.FirstError);
        }

        [Theory]
        [MemberData(nameof(InvalidStatusesForAssignment))]
        public void AssignTeam_WhenNotReportedOrInProgress_ReturnsError(Domain.Disasters.Disaster disaster)
        {
            var result = disaster.AssignTeam(Guid.NewGuid());

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.CannotAssignTeamWhenNotInValidStatus, result.FirstError);
        }

        [Fact]
        public void AssignTeam_WhenAlreadyAssigned_ReturnsTeamAlreadyAssignedError()
        {
            var disaster = DisasterTestFactory.CreateReported();
            var teamId = Guid.NewGuid();
            disaster.AssignTeam(teamId);

            var result = disaster.AssignTeam(teamId);

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.TeamAlreadyAssigned, result.FirstError);
            Assert.Single(disaster.Teams);
        }

        // ---------- RemoveTeam ----------

        [Fact]
        public void RemoveTeam_WhenAssignedAndReported_RemovesTeam()
        {
            var disaster = DisasterTestFactory.CreateReported();
            var teamId = Guid.NewGuid();
            disaster.AssignTeam(teamId);

            var result = disaster.RemoveTeam(teamId);

            Assert.False(result.IsError);
            Assert.Empty(disaster.Teams);
        }

        [Fact]
        public void RemoveTeam_WhenNotAssigned_ReturnsTeamNotFoundError()
        {
            var disaster = DisasterTestFactory.CreateReported();

            var result = disaster.RemoveTeam(Guid.NewGuid());

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.TeamNotFound, result.FirstError);
        }

        [Fact]
        public void RemoveTeam_WhenNotReported_ReturnsCannotRemoveVolunteerWhenNotInReportedStatusError()
        {
            var disaster = DisasterTestFactory.CreateReported();
            var teamId = Guid.NewGuid();
            disaster.AssignTeam(teamId);
            disaster.StartResponse();

            var result = disaster.RemoveTeam(teamId);

            Assert.True(result.IsError);
            // Note: the domain currently reuses the volunteer-removal error here.
            Assert.Equal(DisasterErrors.CannotRemoveVolunteerWhenNotInReportedStatus, result.FirstError);
            Assert.Single(disaster.Teams);
        }

        [Fact]
        public void DispatchResource_NewResource_WhenTeamAssignedAndValidStatus_AddsResourceAndRaisesEvent()
        {
            var disaster = DisasterTestFactory.CreateReported();
            var teamId = Guid.NewGuid();
            var resourceId = Guid.NewGuid();
            disaster.AssignTeam(teamId);
            var assignedAt = DateTimeOffset.UtcNow;

            var result = disaster.DispatchResource(resourceId, teamId, ResourceType.WaterSupplies, 50, assignedAt, "initial supply");

            Assert.False(result.IsError);
            Assert.True(result.Value.IsNew);
            Assert.Equal(50, result.Value.Resource.QuantitySent);
            Assert.Single(disaster.Resources);

            var raised = Assert.Single(disaster.DomainEvents.OfType<ResourceDispatchedToDisaster>());
            Assert.Equal(disaster.Id, raised.DisasterId);
            Assert.Equal(resourceId, raised.ResourceId);
            Assert.Equal(teamId, raised.TeamId);
            Assert.Equal(50, raised.Quantity);
            Assert.Equal(ResourceType.WaterSupplies, raised.ResourceType);
        }

        [Fact]
        public void DispatchResource_ExistingResource_IncreasesQuantityAndReturnsIsNewFalseWithoutNewEvent()
        {
            var disaster = DisasterTestFactory.CreateReported();
            var teamId = Guid.NewGuid();
            var resourceId = Guid.NewGuid();
            disaster.AssignTeam(teamId);
            disaster.DispatchResource(resourceId, teamId, ResourceType.WaterSupplies, 50, DateTimeOffset.UtcNow);
            disaster.ClearDomainEvents();

            var result = disaster.DispatchResource(resourceId, teamId, ResourceType.WaterSupplies, 20, DateTimeOffset.UtcNow);

            Assert.False(result.IsError);
            Assert.False(result.Value.IsNew);
            Assert.Equal(70, result.Value.Resource.QuantitySent);
            Assert.Single(disaster.Resources);
            Assert.Empty(disaster.DomainEvents.OfType<ResourceDispatchedToDisaster>());
        }

        [Fact]
        public void DispatchResource_WithEmptyResourceId_ReturnsIdMustBeProvidedError()
        {
            var disaster = DisasterTestFactory.CreateReported();
            var teamId = Guid.NewGuid();
            disaster.AssignTeam(teamId);

            var result = disaster.DispatchResource(Guid.Empty, teamId, ResourceType.WaterSupplies, 10, DateTimeOffset.UtcNow);

            Assert.True(result.IsError);
            Assert.Equal(DomainErrors.IdMustBeProvided("Resource"), result.FirstError);
        }

        [Fact]
        public void DispatchResource_WithEmptyTeamId_ReturnsIdMustBeProvidedError()
        {
            var disaster = DisasterTestFactory.CreateReported();

            var result = disaster.DispatchResource(Guid.NewGuid(), Guid.Empty, ResourceType.WaterSupplies, 10, DateTimeOffset.UtcNow);

            Assert.True(result.IsError);
            Assert.Equal(DomainErrors.IdMustBeProvided("Team"), result.FirstError);
        }

        [Fact]
        public void DispatchResource_WhenTeamNotAssignedToDisaster_ReturnsTeamNotAssignedToDisasterError()
        {
            var disaster = DisasterTestFactory.CreateReported();

            var result = disaster.DispatchResource(Guid.NewGuid(), Guid.NewGuid(), ResourceType.WaterSupplies, 10, DateTimeOffset.UtcNow);

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.TeamNotAssignedToDisaster, result.FirstError);
        }

        [Fact]
        public void DispatchResource_WhenTeamAssignedButDisasterNoLongerInValidStatus_ReturnsCannotAssignVolunteerWhenNotInValidStatusError()
        {
            var disaster = DisasterTestFactory.CreateReported();
            var teamId = Guid.NewGuid();
            disaster.AssignTeam(teamId);
            disaster.StartResponse();
            disaster.Resolve(DateTimeOffset.UtcNow);

            var result = disaster.DispatchResource(Guid.NewGuid(), teamId, ResourceType.WaterSupplies, 10, DateTimeOffset.UtcNow);

            Assert.True(result.IsError);
            // Note: the domain reuses the volunteer-assignment status error here too.
            Assert.Equal(DisasterErrors.CannotAssignVolunteerWhenNotInValidStatus, result.FirstError);
        }

        [Fact]
        public void DispatchResource_NewResourceWithNonPositiveQuantity_ReturnsResourceQuantityError()
        {
            var disaster = DisasterTestFactory.CreateReported();
            var teamId = Guid.NewGuid();
            disaster.AssignTeam(teamId);

            var result = disaster.DispatchResource(Guid.NewGuid(), teamId, ResourceType.WaterSupplies, 0, DateTimeOffset.UtcNow);

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.ResourceQuantityshouldBeGreaterThanZero, result.FirstError);
            Assert.Empty(disaster.Resources);
        }

        [Fact]
        public void DispatchResource_ExistingResourceWithNonPositiveQuantity_ReturnsResourceQuantityError()
        {
            var disaster = DisasterTestFactory.CreateReported();
            var teamId = Guid.NewGuid();
            var resourceId = Guid.NewGuid();
            disaster.AssignTeam(teamId);
            disaster.DispatchResource(resourceId, teamId, ResourceType.WaterSupplies, 50, DateTimeOffset.UtcNow);

            var result = disaster.DispatchResource(resourceId, teamId, ResourceType.WaterSupplies, 0, DateTimeOffset.UtcNow);

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.ResourceQuantityshouldBeGreaterThanZero, result.FirstError);
            Assert.Equal(50, disaster.Resources.Single(r => r.ResourceId == resourceId).QuantitySent); // unchanged
        }

        // ---------- ConsumeResource ----------

        private static (Domain.Disasters.Disaster disaster, Guid resourceId) ArrangeDisasterWithDispatchedResource(
            ResourceType? resourceType = null, int quantitySent = 20)
        {
            var disaster = DisasterTestFactory.CreateReported();
            var teamId = Guid.NewGuid();
            disaster.AssignTeam(teamId);
            var dispatch = disaster.DispatchResource(
                Guid.NewGuid(), teamId, resourceType ?? ResourceType.MedicalSupplies, quantitySent, DateTimeOffset.UtcNow);
            return (disaster, dispatch.Value.Resource.Id);
        }

        [Fact]
        public void ConsumeResource_WhenResourceExistsAndConsumable_DecreasesRemainingAndRaisesEvent()
        {
            var (disaster, resourceId) = ArrangeDisasterWithDispatchedResource(ResourceType.MedicalSupplies, 20);
            disaster.ClearDomainEvents();

            var result = disaster.ConsumeResource(resourceId, 5);

            Assert.False(result.IsError);
            var resource = disaster.Resources.Single(r => r.Id == resourceId);
            Assert.Equal(5, resource.QuantityConsumed);
            Assert.Equal(15, resource.RemainingQuantity);

            var raised = Assert.Single(disaster.DomainEvents.OfType<ResourceConsumed>());
            Assert.Equal(disaster.Id, raised.DisasterId);
            Assert.Equal(5, raised.Quantity);
        }

        [Fact]
        public void ConsumeResource_WithEmptyResourceId_ReturnsIdMustBeProvidedError()
        {
            var disaster = DisasterTestFactory.CreateReported();

            var result = disaster.ConsumeResource(Guid.Empty, 5);

            Assert.True(result.IsError);
            Assert.Equal(DomainErrors.IdMustBeProvided("Resource"), result.FirstError);
        }

        [Fact]
        public void ConsumeResource_WithNonPositiveQuantity_ReturnsResourceQuantityError()
        {
            var (disaster, resourceId) = ArrangeDisasterWithDispatchedResource();

            var result = disaster.ConsumeResource(resourceId, 0);

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.ResourceQuantityshouldBeGreaterThanZero, result.FirstError);
        }

        [Fact]
        public void ConsumeResource_WhenResourceNotFound_ReturnsResourceNotFoundError()
        {
            var disaster = DisasterTestFactory.CreateReported();

            var result = disaster.ConsumeResource(Guid.NewGuid(), 5);

            Assert.True(result.IsError);
            Assert.Equal(DisasterResourceErrors.ResourceNotFound, result.FirstError);
        }

        [Fact]
        public void ConsumeResource_WhenResourceNotConsumable_ReturnsResourceIsNotConsumableError()
        {
            var (disaster, resourceId) = ArrangeDisasterWithDispatchedResource(ResourceType.Ambulance);

            var result = disaster.ConsumeResource(resourceId, 1);

            Assert.True(result.IsError);
            Assert.Equal(DisasterResourceErrors.ResourceIsNotConsumable, result.FirstError);
        }

        [Fact]
        public void ConsumeResource_WhenQuantityExceedsRemaining_ReturnsResourceConsumptionExceedsSentError()
        {
            var (disaster, resourceId) = ArrangeDisasterWithDispatchedResource(ResourceType.MedicalSupplies, 10);

            var result = disaster.ConsumeResource(resourceId, 11);

            Assert.True(result.IsError);
            Assert.Equal(DisasterResourceErrors.ResourceConsumptionExceedsSent, result.FirstError);
        }

        // ---------- ReturnResource ----------

        [Fact]
        public void ReturnResource_WhenValid_IncreasesReturnedAndRaisesEvent()
        {
            var (disaster, resourceId) = ArrangeDisasterWithDispatchedResource(ResourceType.Stretcher, 10);
            disaster.ClearDomainEvents();

            var result = disaster.ReturnResource(resourceId, 4);

            Assert.False(result.IsError);
            var resource = disaster.Resources.Single(r => r.Id == resourceId);
            Assert.Equal(4, resource.QuantityReturned);

            var raised = Assert.Single(disaster.DomainEvents.OfType<ResourceReturned>());
            Assert.Equal(4, raised.Quantity);
        }

        [Fact]
        public void ReturnResource_WithEmptyId_ReturnsIdMustBeProvidedError()
        {
            var disaster = DisasterTestFactory.CreateReported();

            var result = disaster.ReturnResource(Guid.Empty, 1);

            Assert.True(result.IsError);
            Assert.Equal(DomainErrors.IdMustBeProvided("DisasterResource"), result.FirstError);
        }

        [Fact]
        public void ReturnResource_WithNonPositiveQuantity_ReturnsResourceQuantityError()
        {
            var (disaster, resourceId) = ArrangeDisasterWithDispatchedResource();

            var result = disaster.ReturnResource(resourceId, -1);

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.ResourceQuantityshouldBeGreaterThanZero, result.FirstError);
        }

        [Fact]
        public void ReturnResource_WhenResourceNotFound_ReturnsResourceNotFoundError()
        {
            var disaster = DisasterTestFactory.CreateReported();

            var result = disaster.ReturnResource(Guid.NewGuid(), 1);

            Assert.True(result.IsError);
            Assert.Equal(DisasterResourceErrors.ResourceNotFound, result.FirstError);
        }

        [Fact]
        public void ReturnResource_WhenQuantityExceedsRemaining_ReturnsInvalidReturnQuantityError()
        {
            var (disaster, resourceId) = ArrangeDisasterWithDispatchedResource(ResourceType.Stretcher, 5);

            var result = disaster.ReturnResource(resourceId, 6);

            Assert.True(result.IsError);
            Assert.Equal(DisasterResourceErrors.InvalidReturnQuantity, result.FirstError);
        }

        // ---------- MarkResourceAsDamaged ----------

        [Fact]
        public void MarkResourceAsDamaged_WhenValid_IncreasesDamagedAndRaisesEvent()
        {
            var (disaster, resourceId) = ArrangeDisasterWithDispatchedResource(ResourceType.Tent, 10);
            disaster.ClearDomainEvents();

            var result = disaster.MarkResourceAsDamaged(resourceId, 2);

            Assert.False(result.IsError);
            var resource = disaster.Resources.Single(r => r.Id == resourceId);
            Assert.Equal(2, resource.QuantityDamaged);

            var raised = Assert.Single(disaster.DomainEvents.OfType<ResourceDamaged>());
            Assert.Equal(2, raised.Quantity);
        }

        [Fact]
        public void MarkResourceAsDamaged_WithEmptyId_ReturnsIdMustBeProvidedError()
        {
            var disaster = DisasterTestFactory.CreateReported();

            var result = disaster.MarkResourceAsDamaged(Guid.Empty, 1);

            Assert.True(result.IsError);
            Assert.Equal(DomainErrors.IdMustBeProvided("DisasterResource"), result.FirstError);
        }

        [Fact]
        public void MarkResourceAsDamaged_WithNonPositiveQuantity_ReturnsResourceQuantityError()
        {
            var (disaster, resourceId) = ArrangeDisasterWithDispatchedResource();

            var result = disaster.MarkResourceAsDamaged(resourceId, 0);

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.ResourceQuantityshouldBeGreaterThanZero, result.FirstError);
        }

        [Fact]
        public void MarkResourceAsDamaged_WhenResourceNotFound_ReturnsResourceNotFoundError()
        {
            var disaster = DisasterTestFactory.CreateReported();

            var result = disaster.MarkResourceAsDamaged(Guid.NewGuid(), 1);

            Assert.True(result.IsError);
            Assert.Equal(DisasterResourceErrors.ResourceNotFound, result.FirstError);
        }

        [Fact]
        public void MarkResourceAsDamaged_WhenQuantityExceedsRemaining_ReturnsInvalidDamagedQuantityError()
        {
            var (disaster, resourceId) = ArrangeDisasterWithDispatchedResource(ResourceType.Tent, 3);

            var result = disaster.MarkResourceAsDamaged(resourceId, 4);

            Assert.True(result.IsError);
            Assert.Equal(DisasterResourceErrors.InvalidDamagedQuantity, result.FirstError);
        }




        [Fact]
        public void AddAffectedPersons_WhenResolvedWithValidData_AddsPersonsAndRaisesEvent()
        {
            var disaster = DisasterTestFactory.CreateResolved();
            var people = new[] { ValidPerson("Sara Ali"), ValidPerson("Omar Khaled") };

            var result = disaster.AddAffectedPersons(people);

            Assert.False(result.IsError);
            Assert.Equal(2, result.Value.Count);
            Assert.Equal(2, disaster.AffectedPeople.Count);

            var raised = Assert.Single(disaster.DomainEvents.OfType<AffectedPersonsAdded>());
            Assert.Equal(disaster.Id, raised.DisasterId);
            Assert.Equal(2, raised.PersonIds.Count);
        }

        [Fact]
        public void AddAffectedPersons_WithNullCollection_ReturnsValidationError()
        {
            var disaster = DisasterTestFactory.CreateResolved();

            var result = disaster.AddAffectedPersons(null!);

            Assert.True(result.IsError);
            Assert.Equal("Disaster.AffectedPersons.DataRequired", result.FirstError.Code);
        }

        [Fact]
        public void AddAffectedPersons_WithEmptyCollection_ReturnsValidationError()
        {
            var disaster = DisasterTestFactory.CreateResolved();

            var result = disaster.AddAffectedPersons(Array.Empty<(string, int, string, HealthStatus, string?)>());

            Assert.True(result.IsError);
            Assert.Equal("Disaster.AffectedPersons.DataRequired", result.FirstError.Code);
        }

        [Theory]
        [MemberData(nameof(NonResolvedDisasters))]
        public void AddAffectedPersons_WhenNotResolved_ReturnsCannotAddAffectedPersonsError(Domain.Disasters.Disaster disaster)
        {
            var result = disaster.AddAffectedPersons(new[] { ValidPerson() });

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.CannotAddAffectedPersonsWhenDisasterNotResolved, result.FirstError);
        }

        [Fact]
        public void AddAffectedPersons_WithInvalidPersonData_ReturnsErrorFromAffectedPerson()
        {
            var disaster = DisasterTestFactory.CreateResolved();
            var invalidPerson = (name: "", age: 34, phone: "0999888777", status: HealthStatus.Injured, notes: (string?)null);

            var result = disaster.AddAffectedPersons(new[] { invalidPerson });

            Assert.True(result.IsError);
            Assert.Equal(AffectedPersonErrors.NameRequired, result.FirstError);
            Assert.Empty(disaster.AffectedPeople);
        }

        // ---------- UpdateAffectedPerson ----------

        [Fact]
        public void UpdateAffectedPerson_WhenExistsAndResolved_UpdatesFields()
        {
            var disaster = DisasterTestFactory.CreateResolved();
            var added = disaster.AddAffectedPersons(new[] { ValidPerson() }).Value.Single();

            var result = disaster.UpdateAffectedPerson(added.Id, "Sara Updated", 35, "0999000111", HealthStatus.Evacuated, "moved to shelter");

            Assert.False(result.IsError);
            var updated = disaster.AffectedPeople.Single(p => p.Id == added.Id);
            Assert.Equal("Sara Updated", updated.Name);
            Assert.Equal(35, updated.Age);
            Assert.Equal("0999000111", updated.Phone);
            Assert.Equal(HealthStatus.Evacuated, updated.Status);
            Assert.Equal("moved to shelter", updated.Notes);
        }

        [Fact]
        public void UpdateAffectedPerson_WhenNotFound_ReturnsAffectedPersonNotFoundError()
        {
            var disaster = DisasterTestFactory.CreateResolved();

            var result = disaster.UpdateAffectedPerson(Guid.NewGuid(), "X", 20, "0900000000", HealthStatus.Injured, null);

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.AffectedPeronNotFound, result.FirstError);
        }

        [Fact]
        public void UpdateAffectedPerson_WhenDisasterNoLongerResolved_ReturnsCannotAddAffectedPersonsError()
        {
            var disaster = DisasterTestFactory.CreateResolved();
            var added = disaster.AddAffectedPersons(new[] { ValidPerson() }).Value.Single();
            disaster.Close();

            var result = disaster.UpdateAffectedPerson(added.Id, "X", 20, "0900000000", HealthStatus.Injured, null);

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.CannotAddAffectedPersonsWhenDisasterNotResolved, result.FirstError);
        }

        // ---------- RemoveAffectedPerson ----------

        [Fact]
        public void RemoveAffectedPerson_WhenExistsAndResolved_RemovesPerson()
        {
            var disaster = DisasterTestFactory.CreateResolved();
            var added = disaster.AddAffectedPersons(new[] { ValidPerson() }).Value.Single();

            var result = disaster.RemoveAffectedPerson(added.Id);

            Assert.False(result.IsError);
            Assert.Empty(disaster.AffectedPeople);
        }

        [Fact]
        public void RemoveAffectedPerson_WhenNotFound_ReturnsAffectedPersonNotFoundError()
        {
            var disaster = DisasterTestFactory.CreateResolved();

            var result = disaster.RemoveAffectedPerson(Guid.NewGuid());

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.AffectedPeronNotFound, result.FirstError);
        }

        [Fact]
        public void RemoveAffectedPerson_WhenDisasterNoLongerResolved_ReturnsCannotAddAffectedPersonsError()
        {
            var disaster = DisasterTestFactory.CreateResolved();
            var added = disaster.AddAffectedPersons(new[] { ValidPerson() }).Value.Single();
            disaster.Close();

            var result = disaster.RemoveAffectedPerson(added.Id);

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.CannotAddAffectedPersonsWhenDisasterNotResolved, result.FirstError);
            Assert.Single(disaster.AffectedPeople);
        }


        [Fact]
        public void AddReport_WhenClosedAndNoExistingReport_SetsReportAndRaisesEvent()
        {
            var disaster = DisasterTestFactory.CreateClosed();
            var report = ReportBuilder.Valid().WithDisasterId(disaster.Id).BuildValid();

            var result = disaster.AddReport(report);

            Assert.False(result.IsError);
            Assert.Equal(report, disaster.Report);
            Assert.Equal(report, result.Value);

            var raised = Assert.Single(disaster.DomainEvents.OfType<DisasterReportGenerated>());
            Assert.Equal(disaster.Id, raised.DisasterId);
            Assert.Equal(report.Id, raised.ReportId);
        }

        [Fact]
        public void AddReport_WithNullReport_ReturnsValidationError()
        {
            var disaster = DisasterTestFactory.CreateClosed();

            var result = disaster.AddReport(null!);

            Assert.True(result.IsError);
            Assert.Equal("Report.Required", result.FirstError.Code);
            Assert.Null(disaster.Report);
        }

        [Theory]
        [MemberData(nameof(NonClosedDisasters))]
        public void AddReport_WhenNotClosed_ReturnsCannotGenerateReportWhenDisasterNotClosedError(Domain.Disasters.Disaster disaster)
        {
            var report = ReportBuilder.Valid().WithDisasterId(disaster.Id).BuildValid();

            var result = disaster.AddReport(report);

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.CannotGenerateReportWhenDisasterNotClosed, result.FirstError);
        }

        [Fact]
        public void AddReport_WhenReportAlreadyExists_ReturnsReportAlreadyExistsError()
        {
            var disaster = DisasterTestFactory.CreateClosedWithReport(out var existingReport);
            var anotherReport = ReportBuilder.Valid().WithDisasterId(disaster.Id).BuildValid();

            var result = disaster.AddReport(anotherReport);

            Assert.True(result.IsError);
            Assert.Equal(DisasterErrors.ReportAlreadyExists, result.FirstError);
            Assert.Equal(existingReport, disaster.Report);
        }
    }
}

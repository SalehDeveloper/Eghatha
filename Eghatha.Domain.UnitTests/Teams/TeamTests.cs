using Eghatha.Domain.Shared.Errors;
using Eghatha.Domain.Shared.ValueObjects;
using Eghatha.Domain.Teams;
using Eghatha.Domain.Teams.Events;
using Eghatha.Domain.Teams.TeamMembers;
using Eghatha.Domain.Teams.TeamResources;
using Eghatha.Tests.Common.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.UnitTests.Teams
{
    public class TeamTests
    {
        public static IEnumerable<object[]> NonActiveTeams()
        {
            yield return new object[] { TeamTestFactory.CreateOffDuty() };
            yield return new object[] { TeamTestFactory.CreateOnMission() };
            yield return new object[] { TeamTestFactory.CreateReturning() };
            yield return new object[] { TeamTestFactory.CreateInactive() };
        }

        public static IEnumerable<object[]> ValidStatusTransitions()
        {
            yield return new object[] { TeamStatus.Active, TeamStatus.OffDuty };
            yield return new object[] { TeamStatus.Active, TeamStatus.OnMission };
            yield return new object[] { TeamStatus.Active, TeamStatus.Inactive };
            yield return new object[] { TeamStatus.OffDuty, TeamStatus.Active };
            yield return new object[] { TeamStatus.OffDuty, TeamStatus.OnMission };
            yield return new object[] { TeamStatus.OffDuty, TeamStatus.Inactive };
            yield return new object[] { TeamStatus.OnMission, TeamStatus.OffDuty };
            yield return new object[] { TeamStatus.OnMission, TeamStatus.Returning };
            yield return new object[] { TeamStatus.Inactive, TeamStatus.Active };
            yield return new object[] { TeamStatus.Returning, TeamStatus.Active };
        }

        private static Team CreateInStatus(TeamStatus status)
        {
            if (status == TeamStatus.Active) return TeamTestFactory.CreateActive();
            if (status == TeamStatus.OffDuty) return TeamTestFactory.CreateOffDuty();
            if (status == TeamStatus.OnMission) return TeamTestFactory.CreateOnMission();
            if (status == TeamStatus.Inactive) return TeamTestFactory.CreateInactive();
            if (status == TeamStatus.Returning) return TeamTestFactory.CreateReturning();

            throw new ArgumentOutOfRangeException(nameof(status));
        }

        // ---------- Create ----------

        [Fact]
        public void Create_WithValidData_ReturnsTeamInActiveStatus()
        {
            var location = GeoLocation.Create(36.2021, 37.1343).Value;
            var adminId = Guid.NewGuid();

            var result = TeamBuilder.Valid()
                .WithName("Alpha Rescue")
                .WithSpeciality(TeamSpeciality.SearchAndRescueTeam)
                .WithProvince("Aleppo")
                .WithCity("Al-Bab")
                .WithLocation(location)
                .WithCreatedByAdminId(adminId)
                .Build();

            Assert.False(result.IsError);
            var team = result.Value;
            Assert.Equal("Alpha Rescue", team.Name);
            Assert.Equal(TeamSpeciality.SearchAndRescueTeam, team.Speciality);
            Assert.Equal("Aleppo", team.Province);
            Assert.Equal("Al-Bab", team.City);
            Assert.Equal(location, team.Location);
            Assert.Equal(adminId, team.CreatedByAdminId);
            Assert.Equal(TeamStatus.Active, team.Status);
            Assert.Empty(team.Members);
            Assert.Empty(team.Resources);
            Assert.Null(team.Leader);
        }

        [Fact]
        public void Create_WithEmptyId_ReturnsIdMustBeProvidedError()
        {
            var result = TeamBuilder.Valid().WithId(Guid.Empty).Build();

            Assert.True(result.IsError);
            Assert.Equal(DomainErrors.IdMustBeProvided(nameof(Team)), result.FirstError);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithMissingName_ReturnsNameRequiredError(string? name)
        {
            var result = TeamBuilder.Valid().WithName(name!).Build();

            Assert.True(result.IsError);
            Assert.Equal(TeamErrors.NameRequired, result.FirstError);
        }

        [Fact]
        public void Create_WithNullSpeciality_ReturnsSpecialityRequiredError()
        {
            var result = TeamBuilder.Valid().WithSpeciality(null!).Build();

            Assert.True(result.IsError);
            Assert.Equal(TeamErrors.SpecialityRequired, result.FirstError);
        }

        [Fact]
        public void Create_WithSpecialityNotInList_ReturnsInvalidSpecialityError()
        {
            var fakeSpeciality = new TeamSpeciality("FakeSpeciality", 999);

            var result = TeamBuilder.Valid().WithSpeciality(fakeSpeciality).Build();

            Assert.True(result.IsError);
            Assert.Equal(TeamErrors.InvalidSpeciality, result.FirstError);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithMissingProvince_ReturnsProvinceRequiredError(string? province)
        {
            var result = TeamBuilder.Valid().WithProvince(province!).Build();

            Assert.True(result.IsError);
            Assert.Equal(TeamErrors.ProvinceRequired, result.FirstError);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithMissingCity_ReturnsCityRequiredError(string? city)
        {
            var result = TeamBuilder.Valid().WithCity(city!).Build();

            Assert.True(result.IsError);
            Assert.Equal(TeamErrors.CityRequired, result.FirstError);
        }

        [Fact]
        public void Create_WithNullLocation_ReturnsLocationRequiredError()
        {
            var result = TeamBuilder.Valid().WithLocation(null!).Build();

            Assert.True(result.IsError);
            Assert.Equal(TeamErrors.LocationRequired, result.FirstError);
        }

        [Fact]
        public void Create_WithEmptyCreatedByAdminId_ReturnsCreatedByAdminIdRequiredError()
        {
            var result = TeamBuilder.Valid().WithCreatedByAdminId(Guid.Empty).Build();

            Assert.True(result.IsError);
            Assert.Equal(TeamErrors.CreatedByAdminIdRequired, result.FirstError);
        }

        // ---------- UpdateBaseLocation ----------

        [Fact]
        public void UpdateBaseLocation_WithValidLocation_UpdatesLocationAndRaisesEvent()
        {
            var team = TeamTestFactory.CreateActive();
            var newLocation = GeoLocation.Create(35.0, 38.0).Value;

            var result = team.UpdateBaseLocation(newLocation);

            Assert.False(result.IsError);
            Assert.Equal(newLocation, team.Location);
            var raised = Assert.Single(team.DomainEvents.OfType<TeamLocationChangedEvent>());
            Assert.Equal(team.Id, raised.TeamId);
            Assert.Equal(team.Name, raised.TeamName);
            Assert.Equal(newLocation, raised.GeoLocation);
        }

        [Fact]
        public void UpdateBaseLocation_WithNullLocation_ReturnsLocationRequiredError()
        {
            var team = TeamTestFactory.CreateActive();
            var originalLocation = team.Location;

            var result = team.UpdateBaseLocation(null!);

            Assert.True(result.IsError);
            Assert.Equal(TeamErrors.LocationRequired, result.FirstError);
            Assert.Equal(originalLocation, team.Location);
        }

        // ---------- UpdateStatus ----------

        [Theory]
        [MemberData(nameof(ValidStatusTransitions))]
        public void UpdateStatus_WithValidTransition_UpdatesStatusAndRaisesEvent(TeamStatus from, TeamStatus to)
        {
            var team = CreateInStatus(from);

            var result = team.UpdateStatus(to);

            Assert.False(result.IsError);
            Assert.Equal(to, team.Status);
            var raised = team.DomainEvents.OfType<TeamStatusChangedEvent>().Last();
            Assert.Equal(team.Id, raised.TeamId);
            Assert.Equal(team.Name, raised.TeamName);
            Assert.Equal(to, raised.Status);
        }

        [Theory]
        [InlineData(nameof(TeamStatus.Active), nameof(TeamStatus.Returning))]
        [InlineData(nameof(TeamStatus.OnMission), nameof(TeamStatus.Active))]
        [InlineData(nameof(TeamStatus.OnMission), nameof(TeamStatus.Inactive))]
        [InlineData(nameof(TeamStatus.Inactive), nameof(TeamStatus.OffDuty))]
        [InlineData(nameof(TeamStatus.Inactive), nameof(TeamStatus.OnMission))]
        [InlineData(nameof(TeamStatus.Returning), nameof(TeamStatus.OffDuty))]
        [InlineData(nameof(TeamStatus.Returning), nameof(TeamStatus.OnMission))]
        [InlineData(nameof(TeamStatus.Returning), nameof(TeamStatus.Inactive))]
        public void UpdateStatus_WithInvalidTransition_ReturnsInvalidStatusTransitionError(string fromName, string toName)
        {
            var from = TeamStatus.FromName(fromName);
            var to = TeamStatus.FromName(toName);
            var team = CreateInStatus(from);

            var result = team.UpdateStatus(to);

            Assert.True(result.IsError);
            Assert.Equal(TeamErrors.InvalidStatusTransition(from, to), result.FirstError);
            Assert.Equal(from, team.Status);
        }

        [Fact]
        public void UpdateStatus_WithNullStatus_ReturnsStatusRequiredError()
        {
            var team = TeamTestFactory.CreateActive();

            var result = team.UpdateStatus(null!);

            Assert.True(result.IsError);
            Assert.Equal(TeamErrors.StatusRequired, result.FirstError);
        }

        [Fact]
        public void UpdateStatus_WithStatusNotInList_ReturnsInvalidStatusError()
        {
            var team = TeamTestFactory.CreateActive();
            var fakeStatus = new TeamStatus("FakeStatus", 999);

            var result = team.UpdateStatus(fakeStatus);

            Assert.True(result.IsError);
            Assert.Equal(TeamErrors.InvalidStatus, result.FirstError);
        }

        [Fact]
        public void UpdateStatus_ToInactiveWithActiveMembers_SetsAllMembersInactive()
        {
            var team = TeamTestFactory.CreateActive();
            var m1 = team.AddMember(Guid.NewGuid(), "Medic", false, DateTimeOffset.UtcNow).Value;
            var m2 = team.AddMember(Guid.NewGuid(), "Driver", false, DateTimeOffset.UtcNow).Value;
            team.UpdateMemberStatus(m2.Id, TeamMemberStatus.OffDuty);

            var result = team.UpdateStatus(TeamStatus.Inactive);

            Assert.False(result.IsError);
            Assert.Equal(TeamStatus.Inactive, team.Status);
            Assert.Equal(TeamMemberStatus.Inactive, m1.Status);
            Assert.Equal(TeamMemberStatus.Inactive, m2.Status);
        }

        [Fact]
        public void UpdateStatus_ToInactiveWithMemberOnMission_PropagatesMemberErrorAndLeavesTeamStatusUnchanged()
        {
            // TeamMemberStatusTransitions does not allow OnMission -> Inactive, so this
            // surfaces the member's own error and the team never flips to Inactive.
            var team = TeamTestFactory.CreateActive();
            var member = team.AddMember(Guid.NewGuid(), "Medic", false, DateTimeOffset.UtcNow).Value;
            team.UpdateMemberStatus(member.Id, TeamMemberStatus.OnMission);

            var result = team.UpdateStatus(TeamStatus.Inactive);

            Assert.True(result.IsError);
            Assert.Equal(
                TeamMemberErrors.InvalidStatusTransition(TeamMemberStatus.OnMission, TeamMemberStatus.Inactive),
                result.FirstError);
            Assert.Equal(TeamStatus.Active, team.Status);
            Assert.Equal(TeamMemberStatus.OnMission, member.Status);
        }

        // ---------- Update ----------

        [Fact]
        public void Update_WithAllFieldsProvided_UpdatesAllFields()
        {
            var team = TeamTestFactory.CreateActive();
            var newLocation = GeoLocation.Create(34.0, 39.0).Value;

            var result = team.Update("New Name", TeamSpeciality.MedicalTeam, newLocation, "Homs City", "Homs");

            Assert.False(result.IsError);
            Assert.Equal("New Name", team.Name);
            Assert.Equal(TeamSpeciality.MedicalTeam, team.Speciality);
            Assert.Equal(newLocation, team.Location);
            Assert.Equal("Homs City", team.City);
            Assert.Equal("Homs", team.Province);
        }

        [Fact]
        public void Update_WithAllNullFields_KeepsExistingValues()
        {
            var team = TeamTestFactory.CreateActive();
            var originalName = team.Name;
            var originalSpeciality = team.Speciality;
            var originalLocation = team.Location;
            var originalCity = team.City;
            var originalProvince = team.Province;

            var result = team.Update(null, null, null, null, null);

            Assert.False(result.IsError);
            Assert.Equal(originalName, team.Name);
            Assert.Equal(originalSpeciality, team.Speciality);
            Assert.Equal(originalLocation, team.Location);
            Assert.Equal(originalCity, team.City);
            Assert.Equal(originalProvince, team.Province);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Update_WithBlankName_ReturnsNameRequiredError(string name)
        {
            var team = TeamTestFactory.CreateActive();

            var result = team.Update(name, null, null, null, null);

            Assert.True(result.IsError);
            Assert.Equal(TeamErrors.NameRequired, result.FirstError);
        }

        [Fact]
        public void Update_WithSpecialityNotInList_ReturnsInvalidSpecialityError()
        {
            var team = TeamTestFactory.CreateActive();
            var fakeSpeciality = new TeamSpeciality("FakeSpeciality", 999);

            var result = team.Update(null, fakeSpeciality, null, null, null);

            Assert.True(result.IsError);
            Assert.Equal(TeamErrors.InvalidSpeciality, result.FirstError);
        }

        // ---------- AddMember ----------

        [Fact]
        public void AddMember_WithValidData_RaisesTeamMemeberAddedEventWithCorrectPayload()
        {
            var team = TeamTestFactory.CreateActive();
            var userId = Guid.NewGuid();

            var result = team.AddMember(userId, "Paramedic", false, DateTimeOffset.UtcNow);

            Assert.False(result.IsError);
            var raised = Assert.Single(team.DomainEvents.OfType<TeamMemeberAddedEvent>());
            Assert.Equal(team.Id, raised.TeamId);
            Assert.Equal(team.Name, raised.TeamName);
            Assert.Equal(userId, raised.UserId);
            Assert.Equal(result.Value.Id, raised.MemberId);
        }

        [Fact]
        public void AddMember_WithEmptyUserId_ReturnsIdMustBeProvidedError()
        {
            var team = TeamTestFactory.CreateActive();

            var result = team.AddMember(Guid.Empty, "Paramedic", false, DateTimeOffset.UtcNow);

            Assert.True(result.IsError);
            Assert.Equal(DomainErrors.IdMustBeProvided("User"), result.FirstError);
            Assert.Empty(team.Members);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void AddMember_WithMissingJobTitle_ReturnsJobTitleRequiredError(string? jobTitle)
        {
            var team = TeamTestFactory.CreateActive();

            var result = team.AddMember(Guid.NewGuid(), jobTitle!, false, DateTimeOffset.UtcNow);

            Assert.True(result.IsError);
            Assert.Equal(TeamMemberErrors.JobTitleRequired, result.FirstError);
            Assert.Empty(team.Members);
        }

        [Fact]
        public void AddMember_WhenIsLeaderTrueWithExistingLeader_DemotesPreviousLeaderAndSetsNewOne()
        {
            var team = TeamTestFactory.CreateActiveWithMember(out var firstMember, isLeader: true);

            var result = team.AddMember(Guid.NewGuid(), "Engineer", true, DateTimeOffset.UtcNow);

            Assert.False(result.IsError);
            Assert.False(firstMember.IsLeader);
            Assert.True(result.Value.IsLeader);
            Assert.Equal(result.Value, team.Leader);
        }

        [Fact]
        public void AddMember_WhenIsLeaderFalseWithExistingLeader_KeepsPreviousLeader()
        {
            var team = TeamTestFactory.CreateActiveWithMember(out var firstMember, isLeader: true);

            var result = team.AddMember(Guid.NewGuid(), "Engineer", false, DateTimeOffset.UtcNow);

            Assert.False(result.IsError);
            Assert.False(result.Value.IsLeader);
            Assert.True(firstMember.IsLeader);
            Assert.Equal(firstMember, team.Leader);
        }

        // ---------- UpdateMemberStatus ----------

        [Fact]
        public void UpdateMemberStatus_WithEmptyMemberId_ReturnsIdMustBeProvidedError()
        {
            var team = TeamTestFactory.CreateActive();

            var result = team.UpdateMemberStatus(Guid.Empty, TeamMemberStatus.OffDuty);

            Assert.True(result.IsError);
            Assert.Equal(DomainErrors.IdMustBeProvided("TeamMember"), result.FirstError);
        }

        [Fact]
        public void UpdateMemberStatus_WhenMemberNotFound_ReturnsMemberNotFoundError()
        {
            var team = TeamTestFactory.CreateActive();

            var result = team.UpdateMemberStatus(Guid.NewGuid(), TeamMemberStatus.OffDuty);

            Assert.True(result.IsError);
            Assert.Equal(TeamErrors.MemberNotFound, result.FirstError);
        }

        [Fact]
        public void UpdateMemberStatus_WithValidTransition_UpdatesStatus()
        {
            var team = TeamTestFactory.CreateActiveWithMember(out var member);

            var result = team.UpdateMemberStatus(member.Id, TeamMemberStatus.OffDuty);

            Assert.False(result.IsError);
            Assert.Equal(TeamMemberStatus.OffDuty, member.Status);
        }

        [Fact]
        public void UpdateMemberStatus_WithInvalidTransition_ReturnsUnderlyingInvalidStatusTransitionError()
        {
            var team = TeamTestFactory.CreateActiveWithMember(out var member);
            team.UpdateMemberStatus(member.Id, TeamMemberStatus.OnMission);

            var result = team.UpdateMemberStatus(member.Id, TeamMemberStatus.Inactive);

            Assert.True(result.IsError);
            Assert.Equal(
                TeamMemberErrors.InvalidStatusTransition(TeamMemberStatus.OnMission, TeamMemberStatus.Inactive),
                result.FirstError);
            Assert.Equal(TeamMemberStatus.OnMission, member.Status);
        }

        // ---------- ChangeLeader ----------

        [Fact]
        public void ChangeLeader_WithEmptyMemberId_ReturnsIdMustBeProvidedError()
        {
            var team = TeamTestFactory.CreateActive();

            var result = team.ChangeLeader(Guid.Empty);

            Assert.True(result.IsError);
            Assert.Equal(DomainErrors.IdMustBeProvided("TeamMember"), result.FirstError);
        }

        [Fact]
        public void ChangeLeader_WhenMemberNotFound_ReturnsMemberNotFoundError()
        {
            var team = TeamTestFactory.CreateActive();

            var result = team.ChangeLeader(Guid.NewGuid());

            Assert.True(result.IsError);
            Assert.Equal(TeamErrors.MemberNotFound, result.FirstError);
        }

        [Fact]
        public void ChangeLeader_WhenMemberInactive_ReturnsMemberMustBeActiveToBecomeLeaderError()
        {
            var team = TeamTestFactory.CreateActiveWithMember(out var member);
            team.UpdateMemberStatus(member.Id, TeamMemberStatus.Inactive);

            var result = team.ChangeLeader(member.Id);

            Assert.True(result.IsError);
            Assert.Equal(TeamErrors.MemberMustBeActiveToBecomeLeader, result.FirstError);
            Assert.False(member.IsLeader);
        }

        [Fact]
        public void ChangeLeader_WithActiveMember_SetsLeaderAndDemotesOthers()
        {
            var team = TeamTestFactory.CreateActiveWithMember(out var firstMember, isLeader: true);
            var secondMember = team.AddMember(Guid.NewGuid(), "Engineer", false, DateTimeOffset.UtcNow).Value;

            var result = team.ChangeLeader(secondMember.Id);

            Assert.False(result.IsError);
            Assert.False(firstMember.IsLeader);
            Assert.True(secondMember.IsLeader);
            Assert.Equal(secondMember, team.Leader);
        }

        // ---------- Leader / IsReadyForMission ----------

        [Fact]
        public void Leader_WhenNoLeaderSet_ReturnsNull()
        {
            var team = TeamTestFactory.CreateActiveWithMember(out _);

            Assert.Null(team.Leader);
        }

        [Fact]
        public void Leader_WhenLeaderSet_ReturnsLeaderMember()
        {
            var team = TeamTestFactory.CreateActiveWithMember(out var member, isLeader: true);

            Assert.Equal(member, team.Leader);
        }

        [Fact]
        public void IsReadyForMission_WhenActiveWithActiveMember_ReturnsTrue()
        {
            var team = TeamTestFactory.CreateActiveWithMember(out _);

            Assert.True(team.IsReadyForMission);
        }

        [Fact]
        public void IsReadyForMission_WhenActiveWithNoMembers_ReturnsFalse()
        {
            var team = TeamTestFactory.CreateActive();

            Assert.False(team.IsReadyForMission);
        }

        [Fact]
        public void IsReadyForMission_WhenActiveButNoMemberIsActive_ReturnsFalse()
        {
            var team = TeamTestFactory.CreateActiveWithMember(out var member);
            team.UpdateMemberStatus(member.Id, TeamMemberStatus.OffDuty);

            Assert.False(team.IsReadyForMission);
        }

        [Theory]
        [MemberData(nameof(NonActiveTeams))]
        public void IsReadyForMission_WhenTeamNotActive_ReturnsFalseEvenWithActiveMember(Team team)
        {
            team.AddMember(Guid.NewGuid(), "Medic", false, DateTimeOffset.UtcNow);

            Assert.False(team.IsReadyForMission);
        }

        // ---------- AddResource ----------

        [Fact]
        public void AddResource_WithNewResourceType_CreatesResourceAndReturnsIsNewTrue()
        {
            var team = TeamTestFactory.CreateActive();

            var result = team.AddResource(5, ResourceType.FirstAidKit);

            Assert.False(result.IsError);
            Assert.True(result.Value.IsNew);
            Assert.Equal(ResourceType.FirstAidKit, result.Value.Resource.Type);
            Assert.Equal(5, result.Value.Resource.Quantity);
            Assert.Single(team.Resources);
        }

        [Fact]
        public void AddResource_WithExistingResourceType_IncreasesQuantityAndReturnsIsNewFalse()
        {
            var team = TeamTestFactory.CreateActive();
            var first = team.AddResource(5, ResourceType.FirstAidKit).Value;

            var result = team.AddResource(3, ResourceType.FirstAidKit);

            Assert.False(result.IsError);
            Assert.False(result.Value.IsNew);
            Assert.Equal(first.Resource.Id, result.Value.Resource.Id);
            Assert.Equal(8, result.Value.Resource.Quantity);
            Assert.Single(team.Resources);
        }

        [Fact]
        public void AddResource_WithNonPositiveQuantityForNewType_ReturnsQuantityError()
        {
            var team = TeamTestFactory.CreateActive();

            var result = team.AddResource(0, ResourceType.FirstAidKit);

            Assert.True(result.IsError);
            Assert.Equal(ResourceErrors.QuantityShouldBeGreaterThanZero, result.FirstError);
            Assert.Empty(team.Resources);
        }

        [Fact]
        public void AddResource_WithNonPositiveQuantityForExistingType_DoesNotChangeQuantityButStillSucceeds()
        {
            // NOTE: for an existing resource type, AddResource calls IncreaseQuantity but
            // never checks its ErrorOr result, so a non-positive amount is silently
            // ignored instead of rejected. This test documents that current behavior.
            var team = TeamTestFactory.CreateActive();
            var first = team.AddResource(5, ResourceType.FirstAidKit).Value;

            var result = team.AddResource(0, ResourceType.FirstAidKit);

            Assert.False(result.IsError);
            Assert.Equal(5, first.Resource.Quantity);
        }

        // ---------- IncreaseResourceQuantity ----------

        [Fact]
        public void IncreaseResourceQuantity_WhenResourceNotFound_ReturnsNotFoundError()
        {
            var team = TeamTestFactory.CreateActive();

            var result = team.IncreaseResourceQuantity(Guid.NewGuid(), 5);

            Assert.True(result.IsError);
            Assert.Equal(ResourceErrors.NotFound, result.FirstError);
        }

        [Fact]
        public void IncreaseResourceQuantity_WithValidAmount_IncreasesQuantity()
        {
            var team = TeamTestFactory.CreateActive();
            var resource = team.AddResource(5, ResourceType.FirstAidKit).Value.Resource;

            var result = team.IncreaseResourceQuantity(resource.Id, 3);

            Assert.False(result.IsError);
            Assert.Equal(8, resource.Quantity);
        }

        [Fact]
        public void IncreaseResourceQuantity_WithNonPositiveAmount_ReturnsQuantityErrorAndLeavesQuantityUnchanged()
        {
            var team = TeamTestFactory.CreateActive();
            var resource = team.AddResource(5, ResourceType.FirstAidKit).Value.Resource;

            var result = team.IncreaseResourceQuantity(resource.Id, 0);

            Assert.True(result.IsError);
            Assert.Equal(ResourceErrors.QuantityShouldBeGreaterThanZero, result.FirstError);
            Assert.Equal(5, resource.Quantity);
        }

        // ---------- DecreaseResourceQuantity ----------

        [Fact]
        public void DecreaseResourceQuantity_WhenResourceNotFound_ReturnsNotFoundError()
        {
            var team = TeamTestFactory.CreateActive();

            var result = team.DecreaseResourceQuantity(Guid.NewGuid(), 1);

            Assert.True(result.IsError);
            Assert.Equal(ResourceErrors.NotFound, result.FirstError);
        }

        [Fact]
        public void DecreaseResourceQuantity_WithValidAmount_DecreasesQuantity()
        {
            var team = TeamTestFactory.CreateActive();
            var resource = team.AddResource(10, ResourceType.FirstAidKit).Value.Resource;

            var result = team.DecreaseResourceQuantity(resource.Id, 4);

            Assert.False(result.IsError);
            Assert.Equal(6, resource.Quantity);
        }

        [Fact]
        public void DecreaseResourceQuantity_WithAmountGreaterThanQuantity_ReturnsNotEnoughResourcesError()
        {
            var team = TeamTestFactory.CreateActive();
            var resource = team.AddResource(5, ResourceType.FirstAidKit).Value.Resource;

            var result = team.DecreaseResourceQuantity(resource.Id, 10);

            Assert.True(result.IsError);
            Assert.Equal(ResourceErrors.NotEnoughResources, result.FirstError);
            Assert.Equal(5, resource.Quantity);
        }

        [Fact]
        public void DecreaseResourceQuantity_WithNonPositiveAmount_ReturnsQuantityError()
        {
            var team = TeamTestFactory.CreateActive();
            var resource = team.AddResource(5, ResourceType.FirstAidKit).Value.Resource;

            var result = team.DecreaseResourceQuantity(resource.Id, 0);

            Assert.True(result.IsError);
            Assert.Equal(ResourceErrors.QuantityShouldBeGreaterThanZero, result.FirstError);
        }

        // ---------- DeductResource ----------

        [Fact]
        public void DeductResource_WhenResourceNotFound_ReturnsNotFoundError()
        {
            var team = TeamTestFactory.CreateActive();

            var result = team.DeductResource(Guid.NewGuid(), 1);

            Assert.True(result.IsError);
            Assert.Equal(ResourceErrors.NotFound, result.FirstError);
        }

        [Fact]
        public void DeductResource_WithSufficientQuantity_DecreasesQuantityAndReturnsResource()
        {
            var team = TeamTestFactory.CreateActive();
            var resource = team.AddResource(10, ResourceType.FirstAidKit).Value.Resource;

            var result = team.DeductResource(resource.Id, 4);

            Assert.False(result.IsError);
            Assert.Equal(resource, result.Value);
            Assert.Equal(6, resource.Quantity);
        }

        [Fact]
        public void DeductResource_WithInsufficientQuantity_ReturnsNotEnoughResourcesErrorAndLeavesQuantityUnchanged()
        {
            var team = TeamTestFactory.CreateActive();
            var resource = team.AddResource(5, ResourceType.FirstAidKit).Value.Resource;

            var result = team.DeductResource(resource.Id, 10);

            Assert.True(result.IsError);
            Assert.Equal(ResourceErrors.NotEnoughResources, result.FirstError);
            Assert.Equal(5, resource.Quantity);
        }

        // ---------- ReturnResource ----------

        [Fact]
        public void ReturnResource_WhenResourceNotFound_ReturnsNotFoundError()
        {
            var team = TeamTestFactory.CreateActive();

            var result = team.ReturnResource(Guid.NewGuid(), 1);

            Assert.True(result.IsError);
            Assert.Equal(ResourceErrors.NotFound, result.FirstError);
        }

        [Fact]
        public void ReturnResource_WithValidAmount_IncreasesQuantity()
        {
            var team = TeamTestFactory.CreateActive();
            var resource = team.AddResource(5, ResourceType.FirstAidKit).Value.Resource;
            team.DeductResource(resource.Id, 3);

            var result = team.ReturnResource(resource.Id, 3);

            Assert.False(result.IsError);
            Assert.Equal(5, resource.Quantity);
        }

        [Fact]
        public void ReturnResource_WithNonPositiveAmount_StillReturnsUpdatedButLeavesQuantityUnchanged()
        {
            // NOTE: like AddResource, ReturnResource never checks IncreaseQuantity's
            // ErrorOr result, so an invalid amount is silently ignored while the method
            // still reports success. This test documents that current behavior.
            var team = TeamTestFactory.CreateActive();
            var resource = team.AddResource(5, ResourceType.FirstAidKit).Value.Resource;

            var result = team.ReturnResource(resource.Id, 0);

            Assert.False(result.IsError);
            Assert.Equal(5, resource.Quantity);
        }
    }
}

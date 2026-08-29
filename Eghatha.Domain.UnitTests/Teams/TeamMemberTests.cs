using Eghatha.Domain.Shared.Errors;
using Eghatha.Domain.Teams.TeamMembers;
using Eghatha.Tests.Common.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.UnitTests.Teams
{
    public class TeamMemberTests
    {
        public static IEnumerable<object[]> ValidStatusTransitions()
        {
            yield return new object[] { TeamMemberStatus.Active, TeamMemberStatus.OffDuty };
            yield return new object[] { TeamMemberStatus.Active, TeamMemberStatus.OnMission };
            yield return new object[] { TeamMemberStatus.Active, TeamMemberStatus.Inactive };
            yield return new object[] { TeamMemberStatus.OffDuty, TeamMemberStatus.Active };
            yield return new object[] { TeamMemberStatus.OffDuty, TeamMemberStatus.OnMission };
            yield return new object[] { TeamMemberStatus.OffDuty, TeamMemberStatus.Inactive };
            yield return new object[] { TeamMemberStatus.OnMission, TeamMemberStatus.OffDuty };
            yield return new object[] { TeamMemberStatus.Inactive, TeamMemberStatus.Active };
        }

        // ---------- Create ----------

        [Fact]
        public void Create_WithValidData_ReturnsActiveTeamMember()
        {
            var userId = Guid.NewGuid();
            var joinedAt = new DateTimeOffset(2026, 1, 1, 8, 0, 0, TimeSpan.Zero);

            var result = TeamMemberBuilder.Valid()
                .WithUserId(userId)
                .WithJobTitle("Paramedic")
                .WithIsLeader(true)
                .WithJoinedAt(joinedAt)
                .Build();

            Assert.False(result.IsError);
            var member = result.Value;
            Assert.Equal(userId, member.UserId);
            Assert.Equal("Paramedic", member.JobTitle);
            Assert.True(member.IsLeader);
            Assert.Equal(joinedAt, member.JoinedAt);
            Assert.Equal(TeamMemberStatus.Active, member.Status);
        }

        [Fact]
        public void Create_WithEmptyId_ReturnsIdMustBeProvidedError()
        {
            var result = TeamMemberBuilder.Valid().WithId(Guid.Empty).Build();

            Assert.True(result.IsError);
            Assert.Equal(DomainErrors.IdMustBeProvided(nameof(TeamMember)), result.FirstError);
        }

        [Fact]
        public void Create_WithEmptyUserId_ReturnsIdMustBeProvidedError()
        {
            var result = TeamMemberBuilder.Valid().WithUserId(Guid.Empty).Build();

            Assert.True(result.IsError);
            Assert.Equal(DomainErrors.IdMustBeProvided("User"), result.FirstError);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithMissingJobTitle_ReturnsJobTitleRequiredError(string? jobTitle)
        {
            var result = TeamMemberBuilder.Valid().WithJobTitle(jobTitle!).Build();

            Assert.True(result.IsError);
            Assert.Equal(TeamMemberErrors.JobTitleRequired, result.FirstError);
        }

        // ---------- SetLeader ----------

        [Fact]
        public void SetLeader_TogglesIsLeader()
        {
            var member = TeamMemberBuilder.Valid().WithIsLeader(false).BuildValid();

            var result = member.SetLeader(true);

            Assert.False(result.IsError);
            Assert.True(member.IsLeader);
        }

        // ---------- UpdateStatus ----------

        [Theory]
        [MemberData(nameof(ValidStatusTransitions))]
        public void UpdateStatus_WithValidTransition_UpdatesStatus(TeamMemberStatus from, TeamMemberStatus to)
        {
            var member = TeamMemberBuilder.Valid().BuildValid();
            if (from != TeamMemberStatus.Active)
                member.UpdateStatus(from);

            var result = member.UpdateStatus(to);

            Assert.False(result.IsError);
            Assert.Equal(to, member.Status);
        }

        [Theory]
        [InlineData(nameof(TeamMemberStatus.OnMission), nameof(TeamMemberStatus.Active))]
        [InlineData(nameof(TeamMemberStatus.OnMission), nameof(TeamMemberStatus.Inactive))]
        [InlineData(nameof(TeamMemberStatus.Inactive), nameof(TeamMemberStatus.OffDuty))]
        [InlineData(nameof(TeamMemberStatus.Inactive), nameof(TeamMemberStatus.OnMission))]
        public void UpdateStatus_WithInvalidTransition_ReturnsInvalidStatusTransitionError(string fromName, string toName)
        {
            var from = TeamMemberStatus.FromName(fromName);
            var to = TeamMemberStatus.FromName(toName);
            var member = TeamMemberBuilder.Valid().BuildValid();
            if (from != TeamMemberStatus.Active)
                member.UpdateStatus(from);

            var result = member.UpdateStatus(to);

            Assert.True(result.IsError);
            Assert.Equal(TeamMemberErrors.InvalidStatusTransition(from, to), result.FirstError);
            Assert.Equal(from, member.Status);
        }

        [Fact]
        public void UpdateStatus_WithNullStatus_ReturnsStatusRequiredError()
        {
            var member = TeamMemberBuilder.Valid().BuildValid();

            var result = member.UpdateStatus(null!);

            Assert.True(result.IsError);
            Assert.Equal(TeamMemberErrors.StatusRequired, result.FirstError);
        }

        [Fact]
        public void UpdateStatus_WithStatusNotInList_ReturnsInvalidStatusError()
        {
            var member = TeamMemberBuilder.Valid().BuildValid();
            var fakeStatus = new TeamMemberStatus("FakeStatus", 999);

            var result = member.UpdateStatus(fakeStatus);

            Assert.True(result.IsError);
            Assert.Equal(TeamMemberErrors.InvalidStatus, result.FirstError);
        }
    }
}

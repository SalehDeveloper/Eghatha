using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Shared.Errors;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Teams.TeamMembers
{
    public sealed class TeamMember : AuditableEntity
    {
        public Guid UserId { get; private set; }

    

        public string JobTitle { get; private set; }

        public bool IsLeader { get; private set; }
        public TeamMemberStatus Status { get; private set; }

        public DateTimeOffset JoinedAt { get; private set; }



        private TeamMember()
        {
        }

        private TeamMember(
            Guid id,
            Guid userId,
            string jobTitle,
            bool isLeader,
            TeamMemberStatus status,
            DateTimeOffset joinedAt)
            : base(id)
        {
            UserId = userId;
            JobTitle = jobTitle;
            IsLeader = isLeader;
            Status = status;
            JoinedAt = joinedAt;
        }


        public static ErrorOr<TeamMember> Create(Guid id,
            Guid userId,
            string jobTitle,
            bool isLeader,
            DateTimeOffset joinedAt
            )


        {
            if (id == Guid.Empty)
                return DomainErrors.IdMustBeProvided(nameof(TeamMember));

            if (userId == Guid.Empty)
                return DomainErrors.IdMustBeProvided("User");

            if (string.IsNullOrWhiteSpace(jobTitle))
                return TeamMemberErrors.JobTitleRequired;

            return new TeamMember(id, userId,  jobTitle, isLeader, TeamMemberStatus.Active, joinedAt);


        }

        public ErrorOr<Updated> SetLeader(bool isLeader)
        {
            IsLeader = isLeader;

            return Result.Updated;
        }

        public ErrorOr<Updated> UpdateStatus(TeamMemberStatus status)
        {
            if (status is null)
                return TeamMemberErrors.StatusRequired;

            if (!TeamMemberStatus.List.Contains(status))
                return TeamMemberErrors.InvalidStatus;

            if (Status == TeamMemberStatus.OnMission && status == TeamMemberStatus.Active)
                return TeamMemberErrors.CannotSetToActiveWhenInMission;

            
            Status = status;

            return Result.Updated;
        }
    }
}
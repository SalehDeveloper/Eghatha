using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Shared.Errors;
using Eghatha.Domain.Shared.ValueObjects;
using Eghatha.Domain.Teams.Resources;
using Eghatha.Domain.Teams.TeamMembers;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Teams
{
    public sealed class Team : AuditableEntity
    {
        public string Name { get; private set; }

        public TeamSpeciality Speciality { get; private set; }

        public string Province { get; private set; }

        public string City { get; private set; }

        public TeamStatus Status { get; private set; }

        public GeoLocation Location { get; private set; }

        public Guid CreatedByAdminId { get; private set; }

        private List<TeamMember> _members = new();
        public IReadOnlyList<TeamMember> Members => _members.AsReadOnly();

        private List<Resource> _resources = new();
        public IReadOnlyList<Resource> Resources => _resources.AsReadOnly();

        private Team()
        {


        }

        private Team(
            Guid id,
            string name,
            TeamSpeciality speciality,
            string province,
            string city,
            GeoLocation location,
            Guid createdByAdminId)
            : base(id)
        {
            Name = name;
            Speciality = speciality;
            Province = province;
            City = city;
            Status = TeamStatus.Active;
            Location = location;
            CreatedByAdminId = createdByAdminId;
        }


        public static ErrorOr<Team> Create(
            Guid id,
            string name,
            TeamSpeciality speciality,
            string province,
            string city,
            GeoLocation location,
            Guid createdByAdminId)
        {
            if (id == Guid.Empty)
                return DomainErrors.IdMustBeProvided(nameof(Team));

            if (string.IsNullOrWhiteSpace(name))
                return TeamErrors.NameRequired;

            if (speciality == null)
                return TeamErrors.SpecialityRequired;

            if (!TeamSpeciality.List.Contains(speciality))
                return TeamErrors.InvalidSpeciality;

            if (string.IsNullOrWhiteSpace(province))
                return TeamErrors.ProvinceRequired;

            if (string.IsNullOrWhiteSpace(city))
                return TeamErrors.CityRequired;

            if (location == null)
                return TeamErrors.LocationRequired;

            if (createdByAdminId == Guid.Empty)
                return TeamErrors.CreatedByAdminIdRequired;


            var team = new Team(id, name, speciality, province, city, location, createdByAdminId);
            return team;


        }

        public ErrorOr<Updated> UpdateLocation(GeoLocation newLocation)
        {
            if (newLocation is null)
                return TeamErrors.LocationRequired;

            Location = newLocation;

            return Result.Updated;


        }

        public ErrorOr<Updated> UpdateStatus(TeamStatus newStatus)
        {
            if (newStatus is null)
                return TeamErrors.StatusRequired;

            if (!TeamStatus.List.Contains(newStatus))
                return TeamErrors.InvalidStatus;

            if (Status == TeamStatus.OnMission && newStatus == TeamStatus.Active)
                return TeamErrors.CannotSetToActiveWhenInMission;

            Status = newStatus;

            return Result.Updated;
        }
        public ErrorOr<Updated> Update(
            string? name,
            TeamSpeciality? speciality,
            string? province,
            string? city
            )
        {
            if (name is not null && string.IsNullOrWhiteSpace(name))
                return TeamErrors.NameRequired;

            if (province is not null && string.IsNullOrWhiteSpace(province))
                return TeamErrors.ProvinceRequired;

            if (city is not null && string.IsNullOrWhiteSpace(city))
                return TeamErrors.CityRequired;

            if (speciality is not null && !TeamSpeciality.List.Contains(speciality))
                return TeamErrors.InvalidSpeciality;

            Name = name ?? Name;
            Speciality = speciality ?? Speciality;
            Province = province ?? Province;
            City = city ?? City;

            return Result.Updated;


        }

        public ErrorOr<Updated> AddMember(Guid userId, string jobTitle, bool isLeader, DateTimeOffset joinedAt)
        {
            if (userId == Guid.Empty)
                return DomainErrors.IdMustBeProvided("User");

            if (isLeader && _members.Any(m => m.IsLeader))
                return TeamErrors.TeamAlreadyHasLeader;

            var member = TeamMember.Create(Guid.NewGuid(), userId, Id, jobTitle, isLeader, joinedAt);

            if (member.IsError)
                return member.Errors;

            _members.Add(member.Value);

            return Result.Updated;
        }

        public ErrorOr<Updated> RemoveMember(Guid memberId)
        {
            if (Status == TeamStatus.OnMission)
                return TeamErrors.CannotRemoveMemberWhenInMission;

            var member = _members.FirstOrDefault(m => m.Id == memberId);

            if (member is null)
                return TeamErrors.MemberNotFound;

            if (member.IsLeader)
                return TeamErrors.CannotRemoveLeader;


            _members.Remove(member);


            return Result.Updated;
        }

        public ErrorOr<Updated> UpdateMemberStatus(Guid memberId, TeamMemberStatus newStatus)
        {
            if (memberId == Guid.Empty)
                return DomainErrors.IdMustBeProvided("TeamMember");

            var member = _members.FirstOrDefault(m => m.Id == memberId);

            if (member is null)
                return TeamErrors.MemberNotFound;

            var updateResult = member.UpdateStatus(newStatus);

            if (updateResult.IsError)
                return updateResult.Errors;

            return Result.Updated;
        }

        public ErrorOr<Updated> ChangeLeader(Guid memberId)
        {
            if (memberId == Guid.Empty)
                return DomainErrors.IdMustBeProvided("TeamMember");

            var member = _members.FirstOrDefault(m => m.Id == memberId);

            if (member is null)
                return TeamErrors.MemberNotFound;


            foreach (var m in _members)
            {
                m.SetLeader(false);

            }

            member.SetLeader(true);

            return Result.Updated;
        }

        public bool IsReadyForMission => Status == TeamStatus.Active && _members.Any(m => m.Status == TeamMemberStatus.Active);
        public TeamMember? Leader => _members.FirstOrDefault(m => m.IsLeader);

        public ErrorOr<Updated> AddResource(int quantity, ResourceType type)
        {

            var existing = _resources.FirstOrDefault(r => r.Type == type);

            if (existing != null)
            {
                existing.IncreaseQuantity(quantity);

                return Result.Updated;

            }

            var newResource = Resource.Create(Guid.NewGuid(), Id, type, quantity);

            if (newResource.IsError)
                return newResource.Errors;

            _resources.Add(newResource.Value);

            return Result.Updated;
        }

        public ErrorOr<Updated> ConsumeResource(Guid resourceId, int quantity)
        {
            var resource = _resources.FirstOrDefault(r => r.Id == resourceId);
          
            if (resource is null)
                return ResourceErrors.NotFound;

            if (resource.Quantity < quantity)
                return ResourceErrors.NotEnoughResources;


            resource.DecreaseQuantity(quantity);

            return Result.Updated;
        }

        public ErrorOr<Updated> ReturnResource(Guid resourceId, int quantity)
        {
            var resource = _resources.FirstOrDefault(r => r.Id == resourceId);

            if (resource is null)
                return ResourceErrors.NotFound;

            resource.IncreaseQuantity(quantity);
            return Result.Updated;

        }
    }
}
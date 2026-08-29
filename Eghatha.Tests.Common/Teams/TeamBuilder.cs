using Eghatha.Domain.Shared.ValueObjects;
using Eghatha.Domain.Teams;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Tests.Common.Teams
{
    /// <summary>
    /// Fluent builder that produces a valid <see cref="Team"/> by default.
    /// Use the With* methods to override individual fields when a test needs
    /// to exercise a specific validation branch (e.g. WithName(null)).
    /// </summary>
    public sealed class TeamBuilder
    {
        private Guid _id = Guid.NewGuid();
        private string _name = "Test Team";
        private TeamSpeciality _speciality = TeamSpeciality.FireFightingTeam;
        private string _province = "Aleppo";
        private string _city = "Al-Bab";
        private GeoLocation _location = GeoLocation.Create(36.2021, 37.1343).Value;
        private Guid _createdByAdminId = Guid.NewGuid();

        public static TeamBuilder Valid() => new();

        public TeamBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public TeamBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public TeamBuilder WithSpeciality(TeamSpeciality speciality)
        {
            _speciality = speciality;
            return this;
        }

        public TeamBuilder WithProvince(string province)
        {
            _province = province;
            return this;
        }

        public TeamBuilder WithCity(string city)
        {
            _city = city;
            return this;
        }

        public TeamBuilder WithLocation(GeoLocation location)
        {
            _location = location;
            return this;
        }

        public TeamBuilder WithCreatedByAdminId(Guid createdByAdminId)
        {
            _createdByAdminId = createdByAdminId;
            return this;
        }

        public ErrorOr<Team> Build() =>
            Team.Create(
                _id,
                _name,
                _speciality,
                _province,
                _city,
                _location,
                _createdByAdminId);

        /// <summary>
        /// Builds and unwraps the result. Only use this in tests where the
        /// input is known-valid (arrange phase) — never in the tests that
        /// are actually asserting on Create's validation behavior.
        /// </summary>
        public Team BuildValid() => Build().Value;
    }
}

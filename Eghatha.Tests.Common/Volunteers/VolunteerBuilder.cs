using Eghatha.Domain.Shared.ValueObjects;
using Eghatha.Domain.Volunteers;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Tests.Common.Volunteers
{
    /// <summary>
    /// Fluent builder that produces a valid <see cref="Volunteer"/> by default.
    /// Use the With* methods to override individual fields when a test needs
    /// to exercise a specific validation branch (e.g. WithProvince(null)).
    /// </summary>
    public sealed class VolunteerBuilder
    {
        private Guid _id = Guid.NewGuid();
        private Guid _userId = Guid.NewGuid();
        private VolunteerStatus _status = VolunteerStatus.Available;
        private VolunteerSpeciality _speciality = VolunteerSpeciality.General;
        private GeoLocation _location = GeoLocation.Create(33.5138, 36.2765).Value; // Damascus coords, arbitrary
        private string _province = "Damascus";
        private string _city = "Al-Bab";
        private int _yearsOfExperience = 3;
        private string _cv = "https://example.com/cv/test.pdf";

        public static VolunteerBuilder Valid() => new();

        public VolunteerBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public VolunteerBuilder WithUserId(Guid userId)
        {
            _userId = userId;
            return this;
        }

        public VolunteerBuilder WithStatus(VolunteerStatus status)
        {
            _status = status;
            return this;
        }

        public VolunteerBuilder WithSpeciality(VolunteerSpeciality speciality)
        {
            _speciality = speciality;
            return this;
        }

        public VolunteerBuilder WithLocation(GeoLocation location)
        {
            _location = location;
            return this;
        }

        public VolunteerBuilder WithProvince(string province)
        {
            _province = province;
            return this;
        }

        public VolunteerBuilder WithCity(string city)
        {
            _city = city;
            return this;
        }

        public VolunteerBuilder WithYearsOfExperience(int yearsOfExperience)
        {
            _yearsOfExperience = yearsOfExperience;
            return this;
        }

        public VolunteerBuilder WithCv(string cv)
        {
            _cv = cv;
            return this;
        }

        public ErrorOr<Volunteer> Build() =>
            Volunteer.Create(
                _id,
                _userId,
                _status,
                _speciality,
                _location,
                _province,
                _city,
                _yearsOfExperience,
                _cv);

        /// <summary>
        /// Builds and unwraps the result. Only use this in tests where the
        /// input is known-valid (arrange phase) — never in the tests that
        /// are actually asserting on Create's validation behavior.
        /// </summary>
        public Volunteer BuildValid() => Build().Value;
    }
}

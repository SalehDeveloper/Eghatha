using Eghatha.Domain.Disasters;
using Eghatha.Domain.Shared.ValueObjects;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Tests.Common.Disasters
{
    /// <summary>
    /// Fluent builder that produces a valid <see cref="Disaster"/> by default.
    /// Use the With* methods to override individual fields when a test needs
    /// to exercise a specific validation branch (e.g. WithTitle(null)).
    ///
    /// NOTE: ReporterInfo is assumed to live in Eghatha.Domain.Shared.ValueObjects
    /// alongside GeoLocation (same as it's used in Disaster.cs). If that's wrong,
    /// just fix the using statement here.
    /// </summary>
    public sealed class DisasterBuilder
    {
        private Guid _id = Guid.NewGuid();
        private DisasterType _type = DisasterType.Fire;
        private string _title = "Test Disaster";
        private string _description = "Test disaster description";
        private GeoLocation _location = GeoLocation.Create(33.5138, 36.2765).Value; // Damascus coords, arbitrary
        private string _province = "Damascus";
        private string _city = "Al-Bab";
        private DateTimeOffset _startTime = new(2026, 1, 1, 8, 0, 0, TimeSpan.Zero);
        private ReporterInfo _reporter = ReporterInfo.Create("Test Reporter", "1234567890", "0999999999").Value;
        private string? _customTypeDescription;

        public static DisasterBuilder Valid() => new();

        public DisasterBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public DisasterBuilder WithType(DisasterType type)
        {
            _type = type;
            return this;
        }

        public DisasterBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public DisasterBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public DisasterBuilder WithLocation(GeoLocation location)
        {
            _location = location;
            return this;
        }

        public DisasterBuilder WithProvince(string province)
        {
            _province = province;
            return this;
        }

        public DisasterBuilder WithCity(string city)
        {
            _city = city;
            return this;
        }

        public DisasterBuilder WithStartTime(DateTimeOffset startTime)
        {
            _startTime = startTime;
            return this;
        }

        public DisasterBuilder WithReporter(ReporterInfo reporter)
        {
            _reporter = reporter;
            return this;
        }

        public DisasterBuilder WithCustomTypeDescription(string? customTypeDescription)
        {
            _customTypeDescription = customTypeDescription;
            return this;
        }

        public ErrorOr<Disaster> Build() =>
            Disaster.Create(
                _id,
                _type,
                _title,
                _description,
                _location,
                _province,
                _city,
                _startTime,
                _reporter,
                _customTypeDescription);

        /// <summary>
        /// Builds and unwraps the result. Only use this in tests where the
        /// input is known-valid (arrange phase) — never in the tests that
        /// are actually asserting on Create's validation behavior.
        /// </summary>
        public Disaster BuildValid() => Build().Value;
    }
}

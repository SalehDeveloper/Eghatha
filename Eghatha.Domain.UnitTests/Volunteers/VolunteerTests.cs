using Eghatha.Domain.Shared.Errors;
using Eghatha.Domain.Shared.ValueObjects;
using Eghatha.Domain.Volunteers;
using Eghatha.Domain.Volunteers.Equipments;
using Eghatha.Domain.Volunteers.Events;
using Eghatha.Tests.Common.Volunteers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.UnitTests.Volunteers
{
    public class VolunteerTests
    {
        // ---------- Create ----------

        [Fact]
        public void Create_WithValidData_ReturnsVolunteerWithExpectedDefaults()
        {
            var userId = Guid.NewGuid();
            var location = GeoLocation.Create(36.2021, 37.1343).Value;

            var result = VolunteerBuilder.Valid()
                .WithUserId(userId)
                .WithStatus(VolunteerStatus.Available)
                .WithSpeciality(VolunteerSpeciality.FirstAid)
                .WithProvince("Aleppo")
                .WithCity("Al-Bab")
                .WithLocation(location)
                .WithYearsOfExperience(4)
                .WithCv("https://example.com/cv/jane.pdf")
                .Build();

            Assert.False(result.IsError);
            var volunteer = result.Value;
            Assert.Equal(userId, volunteer.UserId);
            Assert.Equal(VolunteerStatus.Available, volunteer.Status);
            Assert.Equal(VolunteerSpeciality.FirstAid, volunteer.Speciality);
            Assert.Equal("Aleppo", volunteer.Province);
            Assert.Equal("Al-Bab", volunteer.City);
            Assert.Equal(location, volunteer.Location);
            Assert.Equal(4, volunteer.YearsOfExperience);
            Assert.Equal("https://example.com/cv/jane.pdf", volunteer.Cv);
            Assert.Equal(0, volunteer.TotalMissions);
            Assert.Equal(0, volunteer.TotalScore);
            Assert.Equal(0, volunteer.AverageScore);
            Assert.Empty(volunteer.Equipments);
        }

        [Fact]
        public void Create_WithValidData_RaisesVolunteerCreatedEventWithCorrectPayload()
        {
            var userId = Guid.NewGuid();

            var result = VolunteerBuilder.Valid().WithUserId(userId).Build();

            Assert.False(result.IsError);
            var volunteer = result.Value;
            var raised = Assert.Single(volunteer.DomainEvents.OfType<VolunteerCreated>());
            Assert.Equal(volunteer.Id, raised.VolunteerId);
            Assert.Equal(userId, raised.UserId);
        }

        [Fact]
        public void Create_WithEmptyId_ReturnsIdMustBeProvidedError()
        {
            var result = VolunteerBuilder.Valid().WithId(Guid.Empty).Build();

            Assert.True(result.IsError);
            Assert.Equal(DomainErrors.IdMustBeProvided(nameof(Volunteer)), result.FirstError);
        }

        [Fact]
        public void Create_WithEmptyUserId_ReturnsUserIdRequiredError()
        {
            var result = VolunteerBuilder.Valid().WithUserId(Guid.Empty).Build();

            Assert.True(result.IsError);
            Assert.Equal(DomainErrors.UserIdRequired, result.FirstError);
        }

        [Fact]
        public void Create_WithNullStatus_ReturnsStatusRequiredError()
        {
            var result = VolunteerBuilder.Valid().WithStatus(null!).Build();

            Assert.True(result.IsError);
            Assert.Equal(VolunteerErrors.StatusRequired, result.FirstError);
        }

        [Fact]
        public void Create_WithStatusNotInList_ReturnsStatusInvalidError()
        {
            var fakeStatus = new VolunteerStatus("FakeStatus", 999);

            var result = VolunteerBuilder.Valid().WithStatus(fakeStatus).Build();

            Assert.True(result.IsError);
            Assert.Equal(VolunteerErrors.StatusInvalid, result.FirstError);
        }

        [Fact]
        public void Create_WithNullSpeciality_ReturnsSpecialityInvalidError()
        {
            // NOTE: Create checks `speciality is null` and `TryFromName` with the
            // *same* branch, so a null speciality returns SpecialityInvalid here —
            // not SpecialityRequired. UpdateSpeciality (below) behaves differently
            // for the same null case. This test documents Create's current behavior.
            var result = VolunteerBuilder.Valid().WithSpeciality(null!).Build();

            Assert.True(result.IsError);
            Assert.Equal(VolunteerErrors.SpecialityInvalid, result.FirstError);
        }

        [Fact]
        public void Create_WithSpecialityNotInList_ReturnsSpecialityInvalidError()
        {
            var fakeSpeciality = new VolunteerSpeciality("FakeSpeciality", 999);

            var result = VolunteerBuilder.Valid().WithSpeciality(fakeSpeciality).Build();

            Assert.True(result.IsError);
            Assert.Equal(VolunteerErrors.SpecialityInvalid, result.FirstError);
        }

        [Fact]
        public void Create_WithNullLocation_ReturnsLocationRequiredError()
        {
            var result = VolunteerBuilder.Valid().WithLocation(null!).Build();

            Assert.True(result.IsError);
            Assert.Equal(VolunteerErrors.LocationRequired, result.FirstError);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithMissingProvince_ReturnsProvinceRequiredError(string? province)
        {
            var result = VolunteerBuilder.Valid().WithProvince(province!).Build();

            Assert.True(result.IsError);
            Assert.Equal(VolunteerErrors.ProvinceRequired, result.FirstError);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithMissingCity_ReturnsCityRequiredError(string? city)
        {
            var result = VolunteerBuilder.Valid().WithCity(city!).Build();

            Assert.True(result.IsError);
            Assert.Equal(VolunteerErrors.CityRequired, result.FirstError);
        }

        [Fact]
        public void Create_WithNegativeYearsOfExperience_ReturnsExperienceMustBeGreaterThanZeroError()
        {
            var result = VolunteerBuilder.Valid().WithYearsOfExperience(-1).Build();

            Assert.True(result.IsError);
            Assert.Equal(VolunteerErrors.ExperienceMustBeGreaterThanZero, result.FirstError);
        }

        [Fact]
        public void Create_WithZeroYearsOfExperience_Succeeds()
        {
            var result = VolunteerBuilder.Valid().WithYearsOfExperience(0).Build();

            Assert.False(result.IsError);
            Assert.Equal(0, result.Value.YearsOfExperience);
        }

        // ---------- UpdateStatus ----------

        [Fact]
        public void UpdateStatus_WithNullStatus_ReturnsStatusRequiredError()
        {
            var volunteer = VolunteerTestFactory.CreateValid();

            var result = volunteer.UpdateStatus(null!);

            Assert.True(result.IsError);
            Assert.Equal(VolunteerErrors.StatusRequired, result.FirstError);
        }

        [Fact]
        public void UpdateStatus_WithStatusNotInList_ReturnsStatusInvalidError()
        {
            var volunteer = VolunteerTestFactory.CreateValid();
            var fakeStatus = new VolunteerStatus("FakeStatus", 999);

            var result = volunteer.UpdateStatus(fakeStatus);

            Assert.True(result.IsError);
            Assert.Equal(VolunteerErrors.StatusInvalid, result.FirstError);
            Assert.Equal(VolunteerStatus.Available, volunteer.Status);
        }

        [Theory]
        [InlineData(nameof(VolunteerStatus.UnAvailable))]
        [InlineData(nameof(VolunteerStatus.Busy))]
        [InlineData(nameof(VolunteerStatus.UnderReview))]
        public void UpdateStatus_WithAnyValidStatus_UpdatesStatus(string statusName)
        {
            // NOTE: unlike Team's UpdateStatus, VolunteerStatus has no transition
            // rules — any valid status is accepted from any current status.
            var volunteer = VolunteerTestFactory.CreateValid();
            var target = VolunteerStatus.FromName(statusName);

            var result = volunteer.UpdateStatus(target);

            Assert.False(result.IsError);
            Assert.Equal(target, volunteer.Status);
        }

        // ---------- UpdateSpeciality ----------

        [Fact]
        public void UpdateSpeciality_WithNullSpeciality_ReturnsSpecialityRequiredError()
        {
            // NOTE: unlike Create, UpdateSpeciality returns SpecialityRequired
            // (not SpecialityInvalid) for a null speciality.
            var volunteer = VolunteerTestFactory.CreateValid();

            var result = volunteer.UpdateSpeciality(null!);

            Assert.True(result.IsError);
            Assert.Equal(VolunteerErrors.SpecialityRequired, result.FirstError);
        }

        [Fact]
        public void UpdateSpeciality_WithSpecialityNotInList_ReturnsSpecialityInvalidError()
        {
            var volunteer = VolunteerTestFactory.CreateValid();
            var fakeSpeciality = new VolunteerSpeciality("FakeSpeciality", 999);

            var result = volunteer.UpdateSpeciality(fakeSpeciality);

            Assert.True(result.IsError);
            Assert.Equal(VolunteerErrors.SpecialityInvalid, result.FirstError);
        }

        [Fact]
        public void UpdateSpeciality_WithValidSpeciality_UpdatesSpeciality()
        {
            var volunteer = VolunteerTestFactory.CreateValid();

            var result = volunteer.UpdateSpeciality(VolunteerSpeciality.SearchAndRescue);

            Assert.False(result.IsError);
            Assert.Equal(VolunteerSpeciality.SearchAndRescue, volunteer.Speciality);
        }

        // ---------- UpdateLocation ----------

        [Fact]
        public void UpdateLocation_WithValidLocation_UpdatesLocationProvinceAndCity()
        {
            var volunteer = VolunteerTestFactory.CreateValid();
            var newLocation = GeoLocation.Create(35.0, 38.0).Value;

            var result = volunteer.UpdateLocation(newLocation, "Homs", "Homs City");

            Assert.False(result.IsError);
            Assert.Equal(newLocation, volunteer.Location);
            Assert.Equal("Homs", volunteer.Province);
            Assert.Equal("Homs City", volunteer.City);
        }

        [Fact]
        public void UpdateLocation_WithNullLocation_ReturnsLocationRequiredErrorAndLeavesFieldsUnchanged()
        {
            var volunteer = VolunteerTestFactory.CreateValid();
            var originalLocation = volunteer.Location;
            var originalProvince = volunteer.Province;
            var originalCity = volunteer.City;

            var result = volunteer.UpdateLocation(null!, "Homs", "Homs City");

            Assert.True(result.IsError);
            Assert.Equal(VolunteerErrors.LocationRequired, result.FirstError);
            Assert.Equal(originalLocation, volunteer.Location);
            Assert.Equal(originalProvince, volunteer.Province);
            Assert.Equal(originalCity, volunteer.City);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void UpdateLocation_WithBlankProvinceOrCity_StillSucceeds(string? blank)
        {
            // NOTE: unlike Create, UpdateLocation does not validate province/city —
            // only location is checked. This test documents that current behavior.
            var volunteer = VolunteerTestFactory.CreateValid();
            var newLocation = GeoLocation.Create(35.0, 38.0).Value;

            var result = volunteer.UpdateLocation(newLocation, blank!, blank!);

            Assert.False(result.IsError);
            Assert.Equal(blank, volunteer.Province);
            Assert.Equal(blank, volunteer.City);
        }

        // ---------- UpdateYearsofExperienec ----------

        [Fact]
        public void UpdateYearsofExperienec_WithNegativeValue_ReturnsExperienceMustBeGreaterThanZeroError()
        {
            var volunteer = VolunteerTestFactory.CreateValid();
            var original = volunteer.YearsOfExperience;

            var result = volunteer.UpdateYearsofExperienec(-1);

            Assert.True(result.IsError);
            Assert.Equal(VolunteerErrors.ExperienceMustBeGreaterThanZero, result.FirstError);
            Assert.Equal(original, volunteer.YearsOfExperience);
        }

        [Fact]
        public void UpdateYearsofExperienec_WithNonNegativeValue_UpdatesYearsOfExperience()
        {
            var volunteer = VolunteerTestFactory.CreateValid();

            var result = volunteer.UpdateYearsofExperienec(7);

            Assert.False(result.IsError);
            Assert.Equal(7, volunteer.YearsOfExperience);
        }

        // ---------- UpdateCv ----------

        [Fact]
        public void UpdateCv_WithNewValue_UpdatesCv()
        {
            var volunteer = VolunteerTestFactory.CreateValid();

            var result = volunteer.UpdateCv("https://example.com/cv/new.pdf");

            Assert.False(result.IsError);
            Assert.Equal("https://example.com/cv/new.pdf", volunteer.Cv);
        }

        [Fact]
        public void UpdateCv_WithNull_StillSucceedsAndClearsCv()
        {
            // NOTE: UpdateCv performs no validation at all.
            var volunteer = VolunteerTestFactory.CreateValid();

            var result = volunteer.UpdateCv(null!);

            Assert.False(result.IsError);
            Assert.Null(volunteer.Cv);
        }

        // ---------- AddEquipment ----------

        [Fact]
        public void AddEquipment_WithNewType_CreatesAndAddsEquipment()
        {
            var volunteer = VolunteerTestFactory.CreateValid();

            var result = volunteer.AddEquipment("First Aid Kit", EquipmentCategory.Medical, 5);

            Assert.False(result.IsError);
            Assert.Equal("First Aid Kit", result.Value.Name);
            Assert.Equal(EquipmentCategory.Medical, result.Value.Category);
            Assert.Equal(5, result.Value.Quantity);
            Assert.Single(volunteer.Equipments);
        }

        [Fact]
        public void AddEquipment_WithExistingNameAndCategory_IncreasesQuantityInsteadOfDuplicating()
        {
            var volunteer = VolunteerTestFactory.CreateValid();
            volunteer.AddEquipment("First Aid Kit", EquipmentCategory.Medical, 5);

            var result = volunteer.AddEquipment("first aid kit", EquipmentCategory.Medical, 3);

            Assert.False(result.IsError);
            Assert.Equal(8, result.Value.Quantity);
            Assert.Single(volunteer.Equipments);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void AddEquipment_WithMissingName_ReturnsNameRequiredErrorAndDoesNotAdd(string? name)
        {
            var volunteer = VolunteerTestFactory.CreateValid();

            var result = volunteer.AddEquipment(name!, EquipmentCategory.Medical, 5);

            Assert.True(result.IsError);
            Assert.Equal(EquipmentErrors.NameRequired, result.FirstError);
            Assert.Empty(volunteer.Equipments);
        }

        [Fact]
        public void AddEquipment_WithNonPositiveQuantityForNewType_ReturnsQuantityErrorAndDoesNotAdd()
        {
            var volunteer = VolunteerTestFactory.CreateValid();

            var result = volunteer.AddEquipment("First Aid Kit", EquipmentCategory.Medical, 0);

            Assert.True(result.IsError);
            Assert.Equal(EquipmentErrors.QuantityShouldBeGreaterThanZero, result.FirstError);
            Assert.Empty(volunteer.Equipments);
        }

        [Fact]
        public void AddEquipment_WithNonPositiveQuantityForExistingType_ReturnsQuantityErrorAndLeavesQuantityUnchanged()
        {
            // NOTE: unlike Team.AddResource, Volunteer.AddEquipment DOES check the
            // ErrorOr result of IncreaseQuantity for an existing type, so a
            // non-positive amount is correctly rejected here.
            var volunteer = VolunteerTestFactory.CreateValid();
            var first = volunteer.AddEquipment("First Aid Kit", EquipmentCategory.Medical, 5).Value;

            var result = volunteer.AddEquipment("First Aid Kit", EquipmentCategory.Medical, 0);

            Assert.True(result.IsError);
            Assert.Equal(EquipmentErrors.QuantityShouldBeGreaterThanZero, result.FirstError);
            Assert.Equal(5, first.Quantity);
        }

        // ---------- UpdateEquipment ----------

        [Fact]
        public void UpdateEquipment_WithEmptyEquipmentId_ReturnsIdMustBeProvidedError()
        {
            var volunteer = VolunteerTestFactory.CreateValid();

            var result = volunteer.UpdateEquipment(Guid.Empty, "New Name", null, null, null);

            Assert.True(result.IsError);
            Assert.Equal(DomainErrors.IdMustBeProvided(nameof(Equipment)), result.FirstError);
        }

        [Fact]
        public void UpdateEquipment_WhenEquipmentNotFound_ReturnsNotFoundError()
        {
            var volunteer = VolunteerTestFactory.CreateValid();

            var result = volunteer.UpdateEquipment(Guid.NewGuid(), "New Name", null, null, null);

            Assert.True(result.IsError);
            Assert.Equal(EquipmentErrors.NotFound, result.FirstError);
        }

        [Fact]
        public void UpdateEquipment_WithValidPartialData_UpdatesOnlyProvidedFields()
        {
            var volunteer = VolunteerTestFactory.CreateWithEquipment(out var equipment);

            var result = volunteer.UpdateEquipment(equipment.Id, "Trauma Kit", null, null, 10);

            Assert.False(result.IsError);
            Assert.Equal("Trauma Kit", equipment.Name);
            Assert.Equal(EquipmentCategory.Medical, equipment.Category);
            Assert.Equal(10, equipment.Quantity);
        }

        [Fact]
        public void UpdateEquipment_WithBlankName_PropagatesNameRequiredErrorFromEquipment()
        {
            var volunteer = VolunteerTestFactory.CreateWithEquipment(out var equipment);

            var result = volunteer.UpdateEquipment(equipment.Id, "   ", null, null, null);

            Assert.True(result.IsError);
            Assert.Equal(EquipmentErrors.NameRequired, result.FirstError);
        }

        // ---------- RemoveEquipment ----------

        [Fact]
        public void RemoveEquipment_WhenEquipmentNotFound_ReturnsNotFoundError()
        {
            var volunteer = VolunteerTestFactory.CreateValid();

            var result = volunteer.RemoveEquipment(Guid.NewGuid());

            Assert.True(result.IsError);
            Assert.Equal(EquipmentErrors.NotFound, result.FirstError);
        }

        [Fact]
        public void RemoveEquipment_WithValidId_MarksEquipmentDeleted()
        {
            var volunteer = VolunteerTestFactory.CreateWithEquipment(out var equipment);

            var result = volunteer.RemoveEquipment(equipment.Id);

            Assert.False(result.IsError);
            Assert.True(equipment.IsDeleted);
        }

        [Fact]
        public void RemoveEquipment_WhenAlreadyDeleted_ReturnsAlreadyDeletedError()
        {
            var volunteer = VolunteerTestFactory.CreateWithEquipment(out var equipment);
            volunteer.RemoveEquipment(equipment.Id);

            var result = volunteer.RemoveEquipment(equipment.Id);

            Assert.True(result.IsError);
            Assert.Equal(EquipmentErrors.AlreadyDeleted, result.FirstError);
        }

        // ---------- IncreaseEquipmentQuantity ----------

        [Fact]
        public void IncreaseEquipmentQuantity_WhenEquipmentNotFound_ReturnsNotFoundError()
        {
            var volunteer = VolunteerTestFactory.CreateValid();

            var result = volunteer.IncreaseEquipmentQuantity(Guid.NewGuid(), 5);

            Assert.True(result.IsError);
            Assert.Equal(EquipmentErrors.NotFound, result.FirstError);
        }

        [Fact]
        public void IncreaseEquipmentQuantity_WithValidAmount_IncreasesQuantity()
        {
            var volunteer = VolunteerTestFactory.CreateWithEquipment(out var equipment, quantity: 5);

            var result = volunteer.IncreaseEquipmentQuantity(equipment.Id, 3);

            Assert.False(result.IsError);
            Assert.Equal(8, equipment.Quantity);
        }

        [Fact]
        public void IncreaseEquipmentQuantity_WithNonPositiveAmount_ReturnsQuantityErrorAndLeavesQuantityUnchanged()
        {
            var volunteer = VolunteerTestFactory.CreateWithEquipment(out var equipment, quantity: 5);

            var result = volunteer.IncreaseEquipmentQuantity(equipment.Id, 0);

            Assert.True(result.IsError);
            Assert.Equal(EquipmentErrors.QuantityShouldBeGreaterThanZero, result.FirstError);
            Assert.Equal(5, equipment.Quantity);
        }

        // ---------- DecreaseEquipmentQuantity ----------

        [Fact]
        public void DecreaseEquipmentQuantity_WhenEquipmentNotFound_ReturnsNotFoundError()
        {
            var volunteer = VolunteerTestFactory.CreateValid();

            var result = volunteer.DecreaseEquipmentQuantity(Guid.NewGuid(), 1);

            Assert.True(result.IsError);
            Assert.Equal(EquipmentErrors.NotFound, result.FirstError);
        }

        [Fact]
        public void DecreaseEquipmentQuantity_WithValidAmount_DecreasesQuantity()
        {
            var volunteer = VolunteerTestFactory.CreateWithEquipment(out var equipment, quantity: 10);

            var result = volunteer.DecreaseEquipmentQuantity(equipment.Id, 4);

            Assert.False(result.IsError);
            Assert.Equal(6, equipment.Quantity);
        }

        [Fact]
        public void DecreaseEquipmentQuantity_WithAmountGreaterThanQuantity_ReturnsNotEnoughEquipmentsError()
        {
            var volunteer = VolunteerTestFactory.CreateWithEquipment(out var equipment, quantity: 5);

            var result = volunteer.DecreaseEquipmentQuantity(equipment.Id, 10);

            Assert.True(result.IsError);
            Assert.Equal(EquipmentErrors.NotEnoughEquipments, result.FirstError);
            Assert.Equal(5, equipment.Quantity);
        }

        [Fact]
        public void DecreaseEquipmentQuantity_WithNonPositiveAmount_ReturnsQuantityError()
        {
            var volunteer = VolunteerTestFactory.CreateWithEquipment(out var equipment, quantity: 5);

            var result = volunteer.DecreaseEquipmentQuantity(equipment.Id, 0);

            Assert.True(result.IsError);
            Assert.Equal(EquipmentErrors.QuantityShouldBeGreaterThanZero, result.FirstError);
        }

        // ---------- UpdateEquipmentStatus ----------

        [Fact]
        public void UpdateEquipmentStatus_WhenEquipmentNotFound_ReturnsNotFoundError()
        {
            var volunteer = VolunteerTestFactory.CreateValid();

            var result = volunteer.UpdateEquipmentStatus(Guid.NewGuid(), EquipmentStatus.InValid);

            Assert.True(result.IsError);
            Assert.Equal(EquipmentErrors.NotFound, result.FirstError);
        }

        [Fact]
        public void UpdateEquipmentStatus_WithValidStatus_UpdatesStatus()
        {
            var volunteer = VolunteerTestFactory.CreateWithEquipment(out var equipment);

            var result = volunteer.UpdateEquipmentStatus(equipment.Id, EquipmentStatus.InValid);

            Assert.False(result.IsError);
            Assert.Equal(EquipmentStatus.InValid, equipment.Status);
        }

        [Fact]
        public void UpdateEquipmentStatus_WithStatusNotInList_ReturnsInvalidStatusError()
        {
            var volunteer = VolunteerTestFactory.CreateWithEquipment(out var equipment);
            var fakeStatus = new EquipmentStatus("FakeStatus", 999);

            var result = volunteer.UpdateEquipmentStatus(equipment.Id, fakeStatus);

            Assert.True(result.IsError);
            Assert.Equal(EquipmentErrors.InvalidStatus, result.FirstError);
            Assert.Equal(EquipmentStatus.Valid, equipment.Status);
        }

        // ---------- IncreaseTotalMissions ----------

        [Fact]
        public void IncreaseTotalMissions_IncrementsTotalMissionsByOne()
        {
            var volunteer = VolunteerTestFactory.CreateValid();

            var result = volunteer.IncreaseTotalMissions();

            Assert.False(result.IsError);
            Assert.Equal(1, volunteer.TotalMissions);
        }

        [Fact]
        public void IncreaseTotalMissions_CalledMultipleTimes_AccumulatesCount()
        {
            var volunteer = VolunteerTestFactory.CreateValid();

            volunteer.IncreaseTotalMissions();
            volunteer.IncreaseTotalMissions();
            volunteer.IncreaseTotalMissions();

            Assert.Equal(3, volunteer.TotalMissions);
        }

        // ---------- AddScore ----------

        [Fact]
        public void AddScore_WithNegativeScore_ReturnsScoreMustBeGreaterThanZeroError()
        {
            var volunteer = VolunteerTestFactory.CreateValid();

            var result = volunteer.AddScore(-5);

            Assert.True(result.IsError);
            Assert.Equal(VolunteerErrors.ScoreMustBeGreaterThanZero, result.FirstError);
            Assert.Equal(0, volunteer.TotalScore);
        }

        [Fact]
        public void AddScore_WithPositiveScore_AddsToTotalScore()
        {
            var volunteer = VolunteerTestFactory.CreateValid();

            volunteer.AddScore(10);
            var result = volunteer.AddScore(5);

            Assert.False(result.IsError);
            Assert.Equal(15, volunteer.TotalScore);
        }

        // ---------- AverageScore ----------

        [Fact]
        public void AverageScore_WithNoMissions_ReturnsZero()
        {
            var volunteer = VolunteerTestFactory.CreateValid();
            volunteer.AddScore(10);

            Assert.Equal(0, volunteer.AverageScore);
        }

        [Fact]
        public void AverageScore_WithMissionsAndScore_ReturnsCorrectAverage()
        {
            var volunteer = VolunteerTestFactory.CreateValid();
            volunteer.IncreaseTotalMissions();
            volunteer.IncreaseTotalMissions();
            volunteer.AddScore(15);

            Assert.Equal(7.5, volunteer.AverageScore);
        }
    }
}

using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Shared.Errors;
using Eghatha.Domain.Shared.ValueObjects;
using Eghatha.Domain.Volunteers.Equipments;
using ErrorOr;
using Microsoft.AspNetCore.Http;


namespace Eghatha.Domain.Volunteers
{
    public sealed class Volunteer : AuditableEntity
    {
        public Guid UserId { get; }

        public VolunteerStatus Status { get; private set; }

        public VolunteerSpeciality Speciality { get; private set; }

        public GeoLocation Location { get; private set; }

        public int YearsOfExperience { get; private set; }
        public string? Cv { get; private set; }

        public int TotalMissions { get; private set; }

        public int TotalScore { get; private set; }

        public double AverageScore => TotalMissions == 0 ? 0 : (double)TotalScore / TotalMissions;

        public IEnumerable<Equipment> Equipments => _equipments.AsReadOnly();

        private readonly List<Equipment> _equipments = [];
        private Volunteer()
        {

        }

        private Volunteer(
            Guid id,
            Guid userId,
            VolunteerStatus status,
            VolunteerSpeciality speciality,
            GeoLocation location,
            int yearsOfExperience,
            string cv,
            List<Equipment> equipments) : base(id)
        {
            UserId = userId;
            Status = status;
            Speciality = speciality;
            Location = location;
            YearsOfExperience = yearsOfExperience;
            Cv = cv;
            TotalMissions = 0;
            TotalScore = 0;
            _equipments = equipments;

        }

        public static ErrorOr<Volunteer> Create(Guid id,
            Guid userId,
            VolunteerStatus status,
            VolunteerSpeciality speciality,
            GeoLocation location,
            int yearsOfExperience,
            string cv,
            List<Equipment> equipments
            )
        {
            if (id == Guid.Empty)
                return DomainErrors.IdMustBeProvided;
            if (userId == Guid.Empty)
                return DomainErrors.UserIdRequired;

            if (status is null)
                return VolunteerErrors.StatusRequired;

            if (!VolunteerStatus.TryFromName(status.Name, out _))
                return VolunteerErrors.StatusInvalid;

            if (speciality is null)
                return VolunteerErrors.SpecialityInvalid;

            if (!VolunteerSpeciality.TryFromName(speciality.Name, out _))
                return VolunteerErrors.SpecialityInvalid;

            if (location is null)
                return VolunteerErrors.LocationRequired;

            var geoLocation = GeoLocation.Create(location.Latitude, location.Longitude);

            if (geoLocation.IsError) return geoLocation.Errors;

            if (yearsOfExperience < 0)
                return VolunteerErrors.ExperienceMustBeGreaterThanZero;

            return new Volunteer(
                id,
                userId,
                status,
                speciality,
                geoLocation.Value,
                yearsOfExperience,
                cv,
                equipments
                );
        }


        public ErrorOr<Updated> UpdateStatus(VolunteerStatus status)
        {
            if (status is null)
                return VolunteerErrors.StatusRequired;
            if (!VolunteerStatus.TryFromName(status.Name, out _))
                return VolunteerErrors.StatusInvalid;
            Status = status;
            return Result.Updated;
        }

        public ErrorOr<Updated> UpdateSpeciality(VolunteerSpeciality speciality)
        {
            if (speciality is null)
                return VolunteerErrors.SpecialityRequired;
            if (!VolunteerSpeciality.TryFromName(speciality.Name, out _))
                return VolunteerErrors.SpecialityInvalid;
            Speciality = speciality;
            return Result.Updated;
        }

        public ErrorOr<Updated> UpdateLocation(GeoLocation location)
        {
            if (location is null)
                return VolunteerErrors.LocationRequired;

            var geoLocation = GeoLocation.Create(location.Latitude, location.Longitude);
            if (geoLocation.IsError) return geoLocation.Errors;
            Location = geoLocation.Value;
            return Result.Updated;
        }

        public ErrorOr<Updated> UpdateYearsofExperienec(int yearsOfExperience)
        {
            if (yearsOfExperience < 0)
                return VolunteerErrors.ExperienceMustBeGreaterThanZero;

            YearsOfExperience = yearsOfExperience;
            return Result.Updated;
        }

        public ErrorOr<Updated> UpdateCv(string cv)
        {
            Cv = cv;

            return Result.Updated;
        }

        public ErrorOr<Updated> AssignEquipments(List<Equipment> equipments)
        {
            foreach (var equipment in equipments)
            {  

                var result = _AssignEquipment(equipment);
                if (result.IsError)
                    continue;

            }

            return Result.Updated;
        }
        private ErrorOr<Created> _AssignEquipment(Equipment equipment)
        {
            if (equipment is null)
                return VolunteerErrors.EquipmentRequired;

            if (_equipments.Any(e => e.Name.Equals(equipment.Name , StringComparison.OrdinalIgnoreCase) && e.Category == equipment.Category))
                return VolunteerErrors.EquipmentAlreadyAssigned;

            _equipments.Add(equipment);

            return Result.Created;
        }

        public ErrorOr<Updated> UpdateEquipments(List<Equipment> equipments)
        {
             foreach (var equipment in equipments)
            {
               var existing = _equipments.FirstOrDefault(e => e.Id == equipment.Id);
                if (existing is null)
                    continue;
                var result = existing.Update(
                    name: equipment.Name,
                    category: equipment.Category,
                    status: equipment.Status,
                    quantity: equipment.Quantity
                    );

                if (result.IsError)
                    return result.Errors;
            }

            return Result.Updated;
        }


        public ErrorOr<Deleted> RemoveEquipment(Guid equipmentId)
        {
            var equipment = _equipments.FirstOrDefault(e => e.Id == equipmentId);

            if (equipment is null)
                return EquipmentErrors.NotFound;

            if (equipment.IsDeleted)
                return EquipmentErrors.AlreadyDeleted;

            equipment.Delete();

            return Result.Deleted;


        }

        public ErrorOr<Updated> ChangeEquipmentQuantity(Guid equipmentId, int quantity)
        {
            var equipment = _equipments.FirstOrDefault(e => e.Id == equipmentId);

            if (equipment is null)
                return EquipmentErrors.NotFound;

            var result = equipment.ChangeQuantity(quantity);


            return result == Result.Updated ? result : result.Errors;

        }

        public ErrorOr<Updated> UpdateEquipmentStatus(Guid equipmentId, EquipmentStatus status)
        {
            var equipment = _equipments.FirstOrDefault(e => e.Id == equipmentId);

            if (equipment is null)
                return EquipmentErrors.NotFound;

            var result = equipment.UpdateStatus(status);


            return result == Result.Updated ? result : result.Errors;

        }

        public ErrorOr<Updated> IncreaseTotalMissions()
        {
            TotalMissions += 1;

            return Result.Updated;
        }

        public ErrorOr<Updated> AddScore(int score)
        {
            if (score < 0)
                return VolunteerErrors.ScoreMustBeGreaterThanZero;
          
            TotalScore += score;
            return Result.Updated;
        }
    }
}
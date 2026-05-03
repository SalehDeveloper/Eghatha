using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Shared.Errors;
using Eghatha.Domain.Shared.ValueObjects;
using Eghatha.Domain.Teams;
using Eghatha.Domain.Volunteers.Equipments;
using Eghatha.Domain.Volunteers.Events;
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

        public string Province { get; private set; }

        public string City { get; private set; }

        public int YearsOfExperience { get; private set; }
        public string? Cv { get; private set; }

        public int TotalMissions { get; private set; }

        public int TotalScore { get; private set; }

        public double AverageScore => TotalMissions == 0 ? 0 : (double)TotalScore / TotalMissions;

        public IEnumerable<Equipment> Equipments => _equipments.AsReadOnly();

        private readonly List<Equipment> _equipments = new();
        private Volunteer()
        {

        }

        private Volunteer(
            Guid id,
            Guid userId,
            VolunteerStatus status,
            VolunteerSpeciality speciality,
            GeoLocation location,
            string province,
            string city,
            int yearsOfExperience,
            string cv) : base(id)
        {
            UserId = userId;
            Status = status;
            Speciality = speciality;
            Location = location;
            City= city; 
            Province= province; 
            YearsOfExperience = yearsOfExperience;
            Cv = cv;
            TotalMissions = 0;
            TotalScore = 0;
         

        }

        public static ErrorOr<Volunteer> Create(Guid id,
            Guid userId,
            VolunteerStatus status,
            VolunteerSpeciality speciality,
            GeoLocation location,
            string province,
            string city , 
            int yearsOfExperience,
            string cv)
          
            
        {
            if (id == Guid.Empty)
                return DomainErrors.IdMustBeProvided(nameof(Volunteer));
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

            if (string.IsNullOrWhiteSpace(province))
                return VolunteerErrors.ProvinceRequired;

            if (string.IsNullOrWhiteSpace(city))
                return VolunteerErrors.CityRequired;

            if (yearsOfExperience < 0)
                return VolunteerErrors.ExperienceMustBeGreaterThanZero;


            var volunteer =  new  Volunteer(
                id,
                userId,
                status,
                speciality,
                location,
                province,
                city,
                yearsOfExperience,
                cv);

            volunteer.AddDomainEvent(new VolunteerCreated(volunteer.Id, volunteer.UserId));

            return volunteer;
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

        public ErrorOr<Updated> UpdateLocation(GeoLocation location, string province, string city)
        {
            if (location is null)
                return VolunteerErrors.LocationRequired;

           
            Location = location;
            City = city;
            Province= province;
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

        public ErrorOr<Equipment> AddEquipment(string name, EquipmentCategory category, int quantity)
        {
            var existing = _equipments.FirstOrDefault(e =>
                e.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                e.Category == category);

            if (existing is not null)
            {
                var res = existing.IncreaseQuantity(quantity);
                if (res.IsError)
                    return res.Errors;
                return existing;
            }

            var equipment = Equipment.Create(Guid.NewGuid(), name, category, quantity);

            if (equipment.IsError)
                return equipment.Errors;

            _equipments.Add(equipment.Value);

           

            return equipment.Value;
        }

        public ErrorOr<Updated> UpdateEquipment(
        Guid equipmentId,
        string? name,
        EquipmentCategory? category,
        EquipmentStatus? status,
        int? quantity)
        {
            if (equipmentId == Guid.Empty)
                return DomainErrors.IdMustBeProvided(nameof(Equipment));

            var equipment = _equipments.FirstOrDefault(e => e.Id == equipmentId);

            if (equipment is null)
                return EquipmentErrors.NotFound;

            var result = equipment.Update(
                name,
                category,
                status,
                quantity
            );

            if (result.IsError)
                return result.Errors;

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


        public ErrorOr<Updated> IncreaseEquipmentQuantity(Guid equipmentId, int quantity)
        {
            var equipment = _equipments.FirstOrDefault(e => e.Id == equipmentId);

            if (equipment is null)
                return EquipmentErrors.NotFound;

            return equipment.IncreaseQuantity(quantity);
        }

        public ErrorOr<Updated> DecreaseEquipmentQuantity(Guid equipmentId, int quantity)
        {
            var equipment = _equipments.FirstOrDefault(e => e.Id == equipmentId);

            if (equipment is null)
                return EquipmentErrors.NotFound;

            return equipment.DecreaseQuantity(quantity);
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
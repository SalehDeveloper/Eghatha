using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Volunteers.Dtos;
using Eghatha.Application.Features.Volunteers.Queries.GetTopVolunteers;
using Eghatha.Domain.Volunteers;
using Eghatha.Domain.Volunteers.Equipments;
using Eghatha.Infastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Repositories
{
    public class VolunteerRepository : BaseRepository<Volunteer>, IVolunteerRepository
    {
        public VolunteerRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PaginatedList<VolunteerDto>> GetVolunteersAsync(
            int page,
            int pageSize,
            string? searchTerm,
            string? status,
            string? speciality,
            string? province,
            string? city,
            CancellationToken cancellationToken)
        {
            var query = _context.Set<Volunteer>()
                .AsNoTracking()
                .AsQueryable();

            VolunteerStatus? volunteerStatus = null;
            VolunteerSpeciality? volunteerSpeciality = null;

            if (!string.IsNullOrWhiteSpace(status))
            {
                volunteerStatus = VolunteerStatus.FromName(status, true);
                query = query.Where(v => v.Status == volunteerStatus);
            }

            if (!string.IsNullOrWhiteSpace(speciality))
            {
                volunteerSpeciality = VolunteerSpeciality.FromName(speciality, true);
                query = query.Where(v => v.Speciality == volunteerSpeciality);
            }

            if (!string.IsNullOrWhiteSpace(province))
            {
                var pr = province.Trim();
                query = query.Where(v => EF.Functions.Like(v.Province, $"%{pr}%"));
            }

            if (!string.IsNullOrWhiteSpace(city))
            {
                var ct = city.Trim();
                query = query.Where(v => EF.Functions.Like(v.City, $"%{ct}%"));
            }


            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var st = searchTerm.Trim();

                query = query.Where(v =>
                    _context.Users
                        .Where(u =>
                            EF.Functions.Like(u.Email, $"%{st}%"))
                        .Select(u => u.Id)
                        .Contains(v.UserId)
                );
            }

            var totalCount = await query.CountAsync(cancellationToken);


            var data = await query
                .OrderByDescending(v => v.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(v => new
                {
                    v.Id,
                    v.UserId,
                    Status = v.Status.Name,
                    Speciality = v.Speciality.Name,
                    v.Province,
                    v.City,
                    v.YearsOfExperience,
                    v.AverageScore

                })
                .ToListAsync(cancellationToken);


            var userIds = data.Select(v => v.UserId).Distinct().ToList();

            var users = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.PhoneNumber
                })
                .ToDictionaryAsync(u => u.Id, cancellationToken);


            var items = data.Select(v =>
            {
                users.TryGetValue(v.UserId, out var user);

                var fullName = user != null
                    ? $"{user.FirstName} {user.LastName}"
                    : null;


                return new VolunteerDto(
                    v.Id,
                    fullName,
                    user.Email,
                    user.PhoneNumber,
                    v.Status,
                    v.Speciality,
                    v.Province,
                    v.City,
                    v.YearsOfExperience,
                    v.AverageScore
                );
            }).ToList();

            return new PaginatedList<VolunteerDto>
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Items = items
            };
        }


        public async Task<VolunteerDto?> GetVolunteerDetailsByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var query = from vol in _context.Set<Volunteer>().AsNoTracking()
                        join user in _context.Set<ApplicationUser>() on vol.UserId equals user.Id
                        where vol.Id == id
                        select new VolunteerDto(vol.Id,
                        $"{user.FirstName} {user.LastName}",
                        user.Email,
                        user.PhoneNumber,
                        vol.Status.Name,
                        vol.Speciality.Name,
                        vol.Province,
                        vol.City,
                        vol.YearsOfExperience,
                        vol.AverageScore);



            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Volunteer> GetByIdWithEquipmentsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<Volunteer>()
                .Include(v => v.Equipments)
                .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        }

        public async Task AddEquipmentAsync(Equipment equipment, CancellationToken cancellationToken)
        {
            await _context.Set<Equipment>().AddAsync(equipment, cancellationToken);
        }

        public async Task<PaginatedList<VolunteerEquipmentDto>> GetVolunteerEquipmentsAsync(Guid volunteerId, int page, int pageSize, string? category, CancellationToken cancellationToken)
        {
            var query = _context.Set<Volunteer>()
                .AsNoTracking()
                .Where(v => v.Id == volunteerId)
                .SelectMany(v => v.Equipments)
                .Where(e => e.IsDeleted == false)
                .AsQueryable();


            EquipmentCategory? equipmentCategory = null;
            if (!string.IsNullOrWhiteSpace(category))
            {
                equipmentCategory = EquipmentCategory.FromName(category, true);
                query = query.Where(e => e.Category == equipmentCategory);
            }
              

            var totalCount = await query.CountAsync(cancellationToken);

            var equipments = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new VolunteerEquipmentDto(
                    e.Id,
                    e.Name,
                    e.Category.Name,
                    e.Quantity,
                    e.Status.Name
                ))
                .ToListAsync(cancellationToken);

            return new PaginatedList<VolunteerEquipmentDto>
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Items = equipments
            };
        }

        public async Task<IReadOnlyList<Volunteer>> GetAvailableBySpecialitiesAsync(IReadOnlyList<VolunteerSpeciality> specialities, CancellationToken cancellationToken)
        {

            return await _context.Set<Volunteer>()
                .Include(x => x.Equipments)
                .Where(v =>

                    specialities.Contains(v.Speciality)

                    && v.Status == VolunteerStatus.Available

                    && v.Equipments.Any(e =>
                        !e.IsDeleted &&
                        e.Status == EquipmentStatus.Valid &&
                        e.Quantity > 0)
                )
                .OrderByDescending(v =>
                    v.TotalMissions == 0
                        ? 0
                        : (double)v.TotalScore / v.TotalMissions)
                .ThenByDescending(v => v.YearsOfExperience)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<VolunteerDto>> GetVolunteersDetailsByIdsAsync(List<Guid> ids, CancellationToken cancellationToken)
        {
            var query = from vol in _context.Set<Volunteer>().AsNoTracking()
                        join user in _context.Set<ApplicationUser>() on vol.UserId equals user.Id
                        where ids.Contains(vol.Id)
                        select new VolunteerDto(
                        vol.Id,
                        $"{user.FirstName} {user.LastName}",
                        user.Email,
                        user.PhoneNumber,
                        vol.Status.Name,
                        vol.Speciality.Name,
                        vol.Province,
                        vol.City,
                        vol.YearsOfExperience,
                        vol.AverageScore);

            return await query.ToListAsync();
        }

        public async Task<PaginatedList<VolunteerRankingDto>> GetTopVolunteersAsync(int page, int pageSize, string? province, string? city, string? speciality, double? minAverageScore, VolunteerRankingSortBy sortBy, bool descending, CancellationToken cancellationToken)
        {
            var query = _context.Set<Volunteer>()
                 .AsNoTracking()
                 .AsQueryable();

            // ---------------- FILTERS ----------------

            if (!string.IsNullOrWhiteSpace(province))
            {
                var pr = province.Trim();

                query = query.Where(v =>
                    EF.Functions.Like(v.Province, $"%{pr}%"));
            }

            if (!string.IsNullOrWhiteSpace(city))
            {
                var ci = city.Trim();

                query = query.Where(v =>
                    EF.Functions.Like(v.City, $"%{ci}%"));
            }

            VolunteerSpeciality? volunteerSpeciality = null;

            if (!string.IsNullOrWhiteSpace(speciality))
            {
                volunteerSpeciality = VolunteerSpeciality.FromName(speciality , true);
                query = query.Where(v =>
                    v.Speciality == volunteerSpeciality);
            }

            if (minAverageScore.HasValue)
            {
                query = query.Where(v =>
                    (v.TotalMissions == 0 ? 0 :
                        (double)v.TotalScore / v.TotalMissions) >= minAverageScore.Value);
            }

            // ---------------- SORTING ----------------

            query = sortBy switch
            {
                VolunteerRankingSortBy.TotalMissions =>
                    descending
                        ? query.OrderByDescending(v => v.TotalMissions)
                        : query.OrderBy(v => v.TotalMissions),

                VolunteerRankingSortBy.TotalScore =>
                    descending
                        ? query.OrderByDescending(v => v.TotalScore)
                        : query.OrderBy(v => v.TotalScore),

                _ =>
                    descending
                        ? query.OrderByDescending(v =>
                            v.TotalMissions == 0 ? 0 :
                            (double)v.TotalScore / v.TotalMissions)
                        : query.OrderBy(v =>
                            v.TotalMissions == 0 ? 0 :
                            (double)v.TotalScore / v.TotalMissions)
            };

            // ---------------- COUNT ----------------

            var totalCount = await query.CountAsync(cancellationToken);

            // ---------------- DATA ----------------

            var volunteers = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(v => new
                {
                    v.Id,
                    v.UserId,
                    Speciality = v.Speciality.Name,
                    v.Province,
                    v.City,
                    v.TotalMissions,
                    v.TotalScore,
                    AverageScore =
                        v.TotalMissions == 0 ? 0 :
                        (double)v.TotalScore / v.TotalMissions
                })
                .ToListAsync(cancellationToken);

            // ---------------- USERS ----------------

            var userIds = volunteers
                .Select(v => v.UserId)
                .Distinct()
                .ToList();

            var users = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName
                })
                .ToDictionaryAsync(u => u.Id, cancellationToken);

            // ---------------- MAPPING ----------------

            var items = volunteers
                .Select((v, index) =>
                {
                    users.TryGetValue(v.UserId, out var user);

                    var fullName = user is null
                        ? "Unknown"
                        : $"{user.FirstName} {user.LastName}";

                    return new VolunteerRankingDto(
                        v.Id,
                        fullName,
                        v.Speciality,
                        v.Province,
                        v.City,
                        v.TotalMissions,
                        v.TotalScore,
                        Math.Round(v.AverageScore, 2),
                        ((page - 1) * pageSize) + index + 1
                    );
                })
                .ToList();

            // ---------------- RESULT ----------------

            return new PaginatedList<VolunteerRankingDto>
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Items = items
            };
        }
    }
}
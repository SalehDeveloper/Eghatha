using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Volunteers.Dtos;
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
            VolunteerStatus? status,
            VolunteerSpeciality? speciality,
            string? province,
            string? city,
            CancellationToken cancellationToken)
        {
            var query = _context.Set<Volunteer>()
                .AsNoTracking()
                .AsQueryable();

           
            if (status != null && !string.IsNullOrWhiteSpace(status.Name))
            {
                query = query.Where(v => v.Status == status);
            }

            if (speciality != null && !string.IsNullOrWhiteSpace(speciality.Name))
            {
                query = query.Where(v => v.Speciality == speciality);
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
                    VolunteerStatus.FromName(v.Status),
                    VolunteerSpeciality.FromName(v.Speciality),
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


        public async Task<VolunteerDto?> GetVolunteerDetailsByIdAsync(Guid id , CancellationToken cancellationToken )
        {
            var query = from vol in _context.Set<Volunteer>().AsNoTracking()
                        join user in _context.Set<ApplicationUser>() on vol.UserId equals user.Id
                        where vol.Id == id
                        select new VolunteerDto(vol.Id,
                        $"{user.FirstName} {user.LastName}",
                        user.Email,
                        user.PhoneNumber,
                        vol.Status,
                        vol.Speciality,
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


        public async Task<PaginatedList<VolunteerEquipmentDto>> GetVolunteerEquipmentsAsync(Guid volunteerId,int page,int pageSize,EquipmentCategory? category,CancellationToken cancellationToken)
        {
            var query = _context.Set<Volunteer>()
                .AsNoTracking()
                .Where(v => v.Id == volunteerId)
                .SelectMany(v => v.Equipments)
                .Where(e => e.IsDeleted == false)
                .AsQueryable();

            if (category != null)
                query = query.Where(e => e.Category == category);

            var totalCount = await query.CountAsync(cancellationToken);

            var equipments = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new VolunteerEquipmentDto(
                    e.Id,
                    e.Name,
                    e.Category,
                    e.Quantity,
                    e.Status
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
    }
}
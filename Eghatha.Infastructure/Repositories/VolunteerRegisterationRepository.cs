using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.VolunteerRegisterations.Dtos;
using Eghatha.Domain.Shared.ValueObjects;
using Eghatha.Domain.VolunteerRegisterations;
using Eghatha.Domain.Volunteers;
using Eghatha.Infastructure.Data;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Repositories
{
    public class VolunteerRegisterationRepository : BaseRepository<VolunteerRegisteration>, IVolunteerRegisterationRepository
    {
        public VolunteerRegisterationRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PaginatedList<VolunteerRegisterationDto>> GetRegisterationsAsync(int page, int pageSize, string? SearchTerm,
            RegisterationStatus? Status , CancellationToken cancellationToken )
        {
            var query =
                        from reg in _context.Set<VolunteerRegisteration>().AsNoTracking()
                        join vol in _context.Set<Volunteer>() on reg.VolunteerId equals vol.Id
                        join user in _context.Set<ApplicationUser>() on vol.UserId equals user.Id
                        select new
                        {
                            reg,
                            vol,
                            user
                        };

           
            if (Status != null)
            {
                query = query.Where(x => x.reg.Status == Status);
            }

          
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                var term = SearchTerm.Trim();

                query = query.Where(x =>
                    x.reg.Id.ToString().Contains(term) ||
                    (x.user.FirstName + " " + x.user.LastName).Contains(term)
                );
            }

           
            var totalCount = await query.CountAsync(cancellationToken);

           
            var items = await query
                .OrderByDescending(x => x.reg.RequestedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new VolunteerRegisterationDto(
                    x.reg.Id,
                    x.vol.Id,

                    x.user.FirstName + " " + x.user.LastName,
                    x.user.Email,
                    x.user.PhoneNumber,
                    x.user.PhotoUrl,
                    x.vol.Location.Latitude,
                    x.vol.Location.Longitude,
                    x.vol.YearsOfExperience,
                    x.vol.Speciality,
                    x.vol.Cv,
                    x.reg.Status,
                    x.reg.RequestedAt,
                    x.reg.ReviewedAt,
                    x.reg.RejectionReason
                ))
                .ToListAsync(cancellationToken);

            return new PaginatedList<VolunteerRegisterationDto>
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Items = items
            };
        }



        public async Task<VolunteerRegisterationDto?> GetRegisterationByIdAsync(Guid registerationId , CancellationToken cancellationToken )
        {
            var query =
        from reg in _context.Set<VolunteerRegisteration>().AsNoTracking()
        join vol in _context.Set<Volunteer>() on reg.VolunteerId equals vol.Id
        join user in _context.Set<ApplicationUser>() on vol.UserId equals user.Id
        where reg.Id == registerationId
        select new VolunteerRegisterationDto(
            reg.Id,
            vol.Id,
            user.FirstName + " " + user.LastName,
            user.Email,
            user.PhoneNumber,
            user.PhotoUrl,
            vol.Location.Latitude,
            vol.Location.Longitude,
            vol.YearsOfExperience,
            vol.Speciality,
            vol.Cv,
            reg.Status,
            reg.RequestedAt,
            reg.ReviewedAt,
            reg.RejectionReason
        );

         return   await query.FirstOrDefaultAsync(cancellationToken);

         
        }
    }
}

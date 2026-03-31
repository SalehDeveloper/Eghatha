using Eghatha.Application.Common.Services;
using Eghatha.Application.Features.Dashboards.Dtos;
using Eghatha.Infastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Services
{
    public class DashboardService : IDashboardService
    { 
        private readonly AppDbContext _appDbContext;

        public DashboardService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async  Task<AccountStatisticsDto> GetAccountStatisticsAsync(CancellationToken cancellationToken)
        {
            var adminRoleId = await _appDbContext.Roles
           .Where(r => r.Name == "Admin")
           .Select(r => r.Id)
           .FirstOrDefaultAsync(cancellationToken);

           
            var userStats = await _appDbContext.Users
                .GroupBy(_ => 1)
                .Select(g => new
                {
                    TotalUsers = g.Count(),
                    ActiveUsers = g.Count(x => x.IsActive),
                    InactiveUsers = g.Count(x => !x.IsActive)
                })
                .FirstAsync(cancellationToken);

           
            var totalAdmins = await _appDbContext.UserRoles
                .Where(ur => ur.RoleId == adminRoleId)
                .Select(ur => ur.UserId)
                .Distinct()
                .CountAsync(cancellationToken);

            return new AccountStatisticsDto(
                TotalUsers: userStats.TotalUsers,
                ActiveUsers: userStats.ActiveUsers,
                InActiveUsers: userStats.InactiveUsers,
                TotalAdmins: totalAdmins);
        }
    }
}

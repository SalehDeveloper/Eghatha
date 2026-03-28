using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Interfaces
{
    public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
    {
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken);

        Task<int>  RevokeAllByUserId(Guid userId, CancellationToken cancellationToken);
    }
}

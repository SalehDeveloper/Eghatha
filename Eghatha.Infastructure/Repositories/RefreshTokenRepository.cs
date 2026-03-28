using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Identity;
using Eghatha.Infastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Repositories
{
    public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken)
        {
            return await _context.Set<RefreshToken>().FirstOrDefaultAsync(r => r.Token == token , cancellationToken);
        }

        public async Task<int> RevokeAllByUserId(Guid userId, CancellationToken cancellationToken)
        {
           var tokens = await  _context.Set<RefreshToken>().Where(r => r.UserId == userId && !r.IsRevoked).ToListAsync(cancellationToken);

            foreach(var token in tokens)
            {
                token.Revoke();
            }

            return tokens.Count;
        }
    }
}

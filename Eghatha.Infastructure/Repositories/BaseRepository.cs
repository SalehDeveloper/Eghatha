using Eghatha.Domain.Abstractions;
using Eghatha.Infastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> 
        where T : Entity
    {
        protected AppDbContext _context;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T?> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
             await _context.AddAsync(entity, cancellationToken);

            return entity;
        }

        public async Task<bool> ExistsAsync(Guid Id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().FindAsync(Id, cancellationToken) is not null ? true : false;
        }

        public async Task<T?> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().FindAsync(Id, cancellationToken);
        }

        public void SoftDelete(T entity)
        {
            entity.Delete();
        }

        public void Update(T entity)
        {
           _context.Update(entity);
        }
    }
}

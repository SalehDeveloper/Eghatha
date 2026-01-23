using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Abstractions
{
    public interface IBaseRepository<T> where T : Entity
    {
        public Task<T?> AddAsync(T entity, CancellationToken cancellationToken = default);

        public void Update(T entity);

        public void SoftDelete(T entity);

        public Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        public Task<IReadOnlyList<T>> GetAllAsync(  int pageNumber=1  , int pageSize=10, CancellationToken cancellationToken = default);

        public Task<(IEnumerable<T>, int)> FindAllWithOptionsAsync(
            Expression<Func<T, bool>> criteria,
            QueryOptions options,
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[] includes);

    }
}

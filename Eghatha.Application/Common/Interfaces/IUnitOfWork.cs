using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        Task CompleteAsync(CancellationToken cancellationToken);
    }
}

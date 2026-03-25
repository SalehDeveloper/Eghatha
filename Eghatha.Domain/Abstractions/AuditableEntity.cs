using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Abstractions
{
    public abstract class AuditableEntity: Entity
    {
        protected AuditableEntity()
        {
            
        }

        protected AuditableEntity(Guid id)
            :base(id)
        {
            
        }

        public DateTimeOffset CreatedAt { get;  set; }
        public string? CreatedBy { get;  set; }


        public DateTimeOffset LastModifiedUtc { get;  set; }
        public string? LastModifiedBy { get;  set; }


    }
}

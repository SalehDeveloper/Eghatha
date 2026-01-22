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

        public DateTime CreatedAt { get; protected set; }
        public string? CreatedBy { get; protected set; }


        public DateTime LastModifiedUtc { get; protected set; }
        public string? LastModifiedBy { get; protected set; }


    }
}

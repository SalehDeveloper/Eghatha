using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Abstractions
{
    public abstract class Entity
    {
        public Guid Id { get; }

        public bool IsDeleted { get; private set; }

        private readonly List<DomainEvent> _domainEvents = [];

        [NotMapped]
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected Entity()
        {
            
        }


        protected Entity(Guid id)
        {
            Id = id==Guid.Empty ? Guid.NewGuid() : id;
            IsDeleted = false;
        }

        public void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        public void Delete()
        {
            IsDeleted = true;
        }   

    }
}

using System;
using System.Collections.Generic;
using SGA.Domain.DomainEvents;

namespace SGA.Domain.Base
{
    public abstract class BaseEntity<TId> : IHasDomainEvents
    {
        public TId Id { get; set; }
         
        public DateTime CreatedAt { get;  set; }
        public string CreatedBy { get;  set; } = string.Empty;
        public DateTime? ModifiedAt { get;  set; }
        public string? ModifiedBy { get;  set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get;  set; }
        public string? DeletedBy { get;  set; }

        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        
        public void SetCreationInfo(string createdBy)
        {
            CreatedAt = DateTime.UtcNow;
            CreatedBy = createdBy;
        }
        
        public void SetModificationInfo(string modifiedBy)
        {
            ModifiedAt = DateTime.UtcNow;
            ModifiedBy = modifiedBy;
        }
        
        public void MarkAsDeleted(string deletedBy)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = deletedBy;
        }

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}



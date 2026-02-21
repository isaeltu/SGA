using System;

namespace SGA.Domain.Base
{
    public abstract class BaseEntity<T>
    {
        public T Id { get; protected set; }
        
        public DateTime CreatedAt { get; protected set; }
        public string CreatedBy { get; protected set; } = string.Empty;
        public DateTime? ModifiedAt { get; protected set; }
        public string? ModifiedBy { get; protected set; }
        public bool IsDeleted { get; protected set; }
        public DateTime? DeletedAt { get; protected set; }
        public string? DeletedBy { get; protected set; }
        
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
    }
}



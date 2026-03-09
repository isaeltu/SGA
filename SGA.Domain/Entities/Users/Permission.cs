using SGA.Domain.Base;

namespace SGA.Domain.Entities.Users
{
    public class Permission : BaseEntity<int>, IAuditEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }

        public ICollection<Role> Roles { get; set; } = new List<Role>();

        protected Permission() { }

        public Permission(string name, string? description, string createdBy)
        {
            Name = name.Trim();
            Description = description;
            SetCreationInfo(createdBy);
        }

        public void UpdateDetails(string name, string? description, string modifiedBy)
        {
            Name = name.Trim();
            Description = description;
            SetModificationInfo(modifiedBy);
        }
    }
}
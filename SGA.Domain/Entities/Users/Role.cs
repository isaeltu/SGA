using SGA.Domain.Base;

namespace SGA.Domain.Entities.Users
{
    public class Role : BaseEntity<byte>, IAuditEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }

        public ICollection<PersonRole> PersonRoles { get; set; } = new List<PersonRole>();
        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();

        protected Role() { }

        public Role(string name, string? description, string createdBy)
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
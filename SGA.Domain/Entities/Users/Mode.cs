using SGA.Domain.Base;

namespace SGA.Domain.Entities.Users
{
    public class Mode : BaseEntity<int>, IAuditEntity
    {
        public string Name { get; private set; } = string.Empty;

        public ICollection<College> Colleges { get; set; } = new List<College>();

        protected Mode() { }

        public Mode(string name, string createdBy)
        {
            Name = name.Trim();
            SetCreationInfo(createdBy);
        }

        public void Rename(string name, string modifiedBy)
        {
            Name = name.Trim();
            SetModificationInfo(modifiedBy);
        }
    }
}
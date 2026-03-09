using SGA.Domain.Base;

namespace SGA.Domain.Entities.Users
{
    public class PersonRole : BaseEntity<int>, IAuditEntity
    {
        public int PersonId { get; private set; }
        public byte RoleId { get; private set; }

        public Person Person { get; set; } = null!;
        public Role Role { get; set; } = null!;

        protected PersonRole() { }

        public PersonRole(int personId, byte roleId, string createdBy)
        {
            PersonId = personId;
            RoleId = roleId;
            SetCreationInfo(createdBy);
        }
    }
}
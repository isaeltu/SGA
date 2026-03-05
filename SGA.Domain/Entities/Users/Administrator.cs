using SGA.Domain.Base;
using SGA.Domain.Enums.Users;
using System;

namespace SGA.Domain.Entities.Users
{
    public class Administrator : BaseEntity<int>
    {
        public int PersonId { get; set; }
        public AdminLevel AdminLevel { get;  init; }
        public DateTime? LastLoginAt { get; set; }
        public UserType UserType { get; set; }
        
        protected Administrator() { }
        
        public Administrator(
            int personId,
            AdminLevel adminLevel,
            string createdBy)
           
        {
            PersonId = personId;
            AdminLevel = adminLevel;
            UserType = UserType.Administrator;
        }
        
        public void RecordLogin()
        {
            LastLoginAt = DateTime.UtcNow;
        }
    }
}

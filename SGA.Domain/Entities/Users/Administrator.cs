using SGA.Domain.Enums.Users;
using System;

namespace SGA.Domain.Entities.Users
{
    public class Administrator : Usuario
    {
        public AdminLevel AdminLevel { get; protected set; }
        public DateTime? LastLoginAt { get; protected set; }
        
        protected Administrator() { }
        
        public Administrator(
            string firstName,
            string lastName,
            string email,
            string cedula,
            string phoneNumber,
            int rolId,
            AdminLevel adminLevel,
            string createdBy)
            : base(firstName, lastName, email, cedula, phoneNumber, rolId, UserType.Administrator, createdBy)
        {
            AdminLevel = adminLevel;
        }
        
        public void RecordLogin()
        {
            LastLoginAt = DateTime.UtcNow;
        }
    }
}

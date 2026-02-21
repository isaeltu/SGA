using SGA.Domain.Base;
using System.Collections.Generic;

namespace SGA.Domain.Entities.Users
{
    public class College : BaseEntity<int>
    {
        public string Name { get; protected set; } = string.Empty;
        public string? Address { get; protected set; }
        public string? PhoneNumber { get; protected set; }
        public bool IsActive { get; protected set; }
        
        public ICollection<Student> Students { get; protected set; } = new List<Student>();
        
        protected College() { }
        
        public College(string name, string? address, string? phoneNumber, string createdBy)
        {
            Name = name;
            Address = address;
            PhoneNumber = phoneNumber;
            IsActive = true;
            SetCreationInfo(createdBy);
        }
    }
}
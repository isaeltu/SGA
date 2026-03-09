using SGA.Domain.Base;
using System.Collections.Generic;

namespace SGA.Domain.Entities.Users
{
    public class College : BaseEntity<int>
    {
        public int InstitutionId { get; set; }
        public int? ModeId { get; set; }
        public string Name { get; protected set; } = string.Empty;
        public string? TimeEstimated { get; protected set; }
        public string? Address { get; protected set; }
        public string? PhoneNumber { get; protected set; }
        public bool IsActive { get; protected set; }
        public Mode? Mode { get; set; }
        public Institution? Institution { get; set; }
        
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
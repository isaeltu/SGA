using SGA.Domain.Base;
using System.Collections.Generic;

namespace SGA.Domain.Entities.Users
{
    public class Department : BaseEntity<int>
    {
        public string Name { get; protected set; } = string.Empty;
        public string? Description { get; protected set; }
        public bool IsActive { get; protected set; }
        
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        
        protected Department() { }
        
        public Department(string name, string? description, string createdBy)
        {
            Name = name;
            Description = description;
            IsActive = true;
            SetCreationInfo(createdBy);
        }
    }
}
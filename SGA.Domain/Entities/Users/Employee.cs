using SGA.Domain.Base;
using SGA.Domain.Enums.Users;
using SGA.Domain.ValueObjects.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGA.Domain.Entities.Users
{
    public class Employee : BaseEntity<int>
    {
        public int PersonId { get; set; }
        public int DepartmentId { get; set; }
        
        public EmployeeCode EmployeeCode { get; set; } = null!;
        
        public string? Position { get; set; }
        public DateTimeOffset HireDate { get; set; }

        public UserType UserType { get; set; }
        
        public Department Department { get; set; } = null!;

        public Person Person { get; set; }
        
        protected Employee() {}
        
        public Employee(
            int PersonId,
            int DepartmentId,
            string EmployeeCode,
            string position,
            DateTimeOffset hire
            )
        {
            this.PersonId = PersonId;
            this.DepartmentId = DepartmentId;
            this.EmployeeCode = new EmployeeCode(EmployeeCode);
            this.Position = position;
            this.HireDate = hire;
            UserType = UserType.Employee;
        }
    }
}

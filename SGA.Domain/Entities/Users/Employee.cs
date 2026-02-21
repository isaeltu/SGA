using SGA.Domain.Enums.Users;
using SGA.Domain.ValueObjects.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGA.Domain.Entities.Users
{
    public class Employee : Usuario
    {
        public int DepartmentId { get; protected set; }
        
        public EmployeeCode EmployeeCode { get; protected set; } = null!;
        
        public string? Position { get; protected set; }
        public DateTime HireDate { get; protected set; }
        
        public Department Department { get; protected set; } = null!;
        
        protected Employee() { }
        
        public Employee(
            string firstName,
            string lastName,
            string email,
            string cedula,
            string phoneNumber,
            int rolId,
            string employeeCode,
            int departmentId,
            string? position,
            DateTime hireDate,
            string createdBy)
            : base(firstName, lastName, email, cedula, phoneNumber, rolId, UserType.Employee, createdBy)
        {
            EmployeeCode = new EmployeeCode(employeeCode);
            DepartmentId = departmentId;
            Position = position;
            HireDate = hireDate;
        }
    }
}

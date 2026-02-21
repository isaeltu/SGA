using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGA.Domain.Enums.Users;

namespace SGA.Domain.Entities.Users
{
    public class Operator : Usuario
    {
        public string? AssignedArea { get; protected set; }
        public int ShiftNumber { get; protected set; }
        
        protected Operator() { }
        
        public Operator(
            string firstName,
            string lastName,
            string email,
            string cedula,
            string phoneNumber,
            int rolId,
            string? assignedArea,
            int shiftNumber,
            string createdBy)
            : base(firstName, lastName, email, cedula, phoneNumber, rolId, UserType.Operator, createdBy)
        {
            AssignedArea = assignedArea;
            ShiftNumber = shiftNumber;
        }
    }
}

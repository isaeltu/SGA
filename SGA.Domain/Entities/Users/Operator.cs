using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGA.Domain.Base;
using SGA.Domain.Enums.Users;

namespace SGA.Domain.Entities.Users
{
    public class Operator : BaseEntity<int>
    {
        public int PersonId { get; set; }
        public UserType UserType { get; set; }
        public string? AssignedArea { get; protected set; }
        public int ShiftNumber { get; protected set; }
        public Person person { get; init; }
        protected Operator() { }
        
        public Operator(
            int PersonId,
            string AssignedArea,
            int ShiftNumber
            )
        {
            this.PersonId = PersonId;
            this.AssignedArea = AssignedArea;
            this.ShiftNumber = ShiftNumber;
            UserType = UserType.Operator;
        }
    }
}

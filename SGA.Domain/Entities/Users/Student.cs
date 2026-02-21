using SGA.Domain.Enums.Users;
using SGA.Domain.ValueObjects.Users;
using SGA.Domain.Entities.Authorizations;
using SGA.Domain.Entities.Trips;
using System.Collections.Generic;

namespace SGA.Domain.Entities.Users
{
    public class Student : Usuario
    {
        public int? CollegeId { get; protected set; }
        
        public EnrollmentId EnrollmentId { get; protected set; } = null!;
        
        public string? Period { get; protected set; }
        public string? CareerName { get; protected set; }
        
        public College? College { get; protected set; }
        public ICollection<Reservation> Reservations { get; protected set; } = new List<Reservation>();
        
        protected Student() { }
        
        public Student(
            string firstName,
            string lastName,
            string email,
            string cedula,
            string phoneNumber,
            int rolId,
            string enrollmentId,
            string? careerName,
            string? period,
            int? collegeId,
            string createdBy)
            : base(firstName, lastName, email, cedula, phoneNumber, rolId, UserType.Student, createdBy)
        {
            EnrollmentId = new EnrollmentId(enrollmentId);
            CareerName = careerName;
            Period = period;
            CollegeId = collegeId;
        }
    }
}

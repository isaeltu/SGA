using SGA.Domain.Enums.Users;
using SGA.Domain.ValueObjects.Users;
using SGA.Domain.Entities.Authorizations;
using SGA.Domain.Entities.Trips;
using System.Collections.Generic;
using SGA.Domain.Common;
using SGA.Domain.Events.Users;
using SGA.Domain.Base;
using System.Numerics;

namespace SGA.Domain.Entities.Users
{
    public class Student : BaseEntity<int>, IAuditEntity
    {
        
        public int? CollegeId { get; init;}
        public EnrollmentId EnrollmentId { get; init;} = null!;
        public string? Period { get; init;}
        public string? CareerName { get; init;}
        public College? College { get; init;}
        public Person Person { get; set;}
        public int PersonId { get; set;}
        public ICollection<Authorization> Authorizations { get; set; } = new List<Authorization>();
        public ICollection<Boarding> Boardings { get; set; } = new List<Boarding>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        
        public UserType UserType { get; set;}
        protected Student() { }

        public Student(
           int PersonId,
           int CollegeId,
           string Period,
           string CareerName
           )
        {
            this.PersonId = PersonId;
            this.EnrollmentId = EnrollmentId;
            this.Period = Period;
            this.CareerName = CareerName;
            UserType = UserType.Student;
        }


        
        }
    }
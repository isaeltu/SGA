using System;
using System.Collections.Generic;
using SGA.Domain.Base;
using SGA.Domain.Common;
using SGA.Domain.Entities.Authorizations;
using SGA.Domain.Enums.Users;
using SGA.Domain.Events.Users;
using SGA.Domain.Exceptions.Users;
using SGA.Domain.ValueObjects.Users;
namespace SGA.Domain.Entities.Users
{
    public  class Person : BaseEntity<int>
    {

        public int RolId { get; init; }
        public Email Email { get; init; } = null!;
        public PhoneNumber PhoneNumber { get; init; } = null!;
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        private string? PasswordHash { get; init; }
        public string? Cedula { get; init; }
        public UserStatus Status { get; init; }
        public Rol Rol { get; init; }
        public object Student { get; set; }

        protected Person() { }

        public Person(
            int RolId,
            string Email,
            string PhoneNumber,
            string FirstName,
            string LastName,
            string cedula
            )
        {
            this.RolId = RolId;
            this.Email = new Email(Email);
            this.PhoneNumber = new PhoneNumber(PhoneNumber);
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Cedula = cedula;
            Status = UserStatus.Active;
        }

        public Result<Person>  CreatePerson(
            int RolId,
            string Email,
            string PhoneNumber,
            string FirstName,
            string LastName,
            string cedula)
        {
           var person = new Person(
               RolId,
               Email,
               PhoneNumber, 
               FirstName,
               LastName,
               cedula
               );
            return Result<Person>.Success(person);
        }


    }
}


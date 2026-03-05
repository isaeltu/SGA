using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SGA.Domain.Entities.Users;
using SGA.Domain.ValueObjects.Users;

namespace SGA.Domain.Events.Users
{
    public record UserRegistered(
        string firstName,
        string lastName,
        string email,
        string PhoneNumber
        );
    
    public class StudentRegistered
    {
        string firstName;
        string lastName;
        string email;
        string PhoneNumber;


        public  StudentRegistered(
        string firstName,
        string lastName,
        string email,
        string PhoneNumber)
        {
            this.firstName = firstName;
            this.lastName = lastName; 
            this.email = email;
            this.PhoneNumber = PhoneNumber;
        }


    }

    public class StudentEmailHandler(StudentRegistered @event)
    {
        string dispara = $"El estudiante se registro correctamente{@event}";
    }
}

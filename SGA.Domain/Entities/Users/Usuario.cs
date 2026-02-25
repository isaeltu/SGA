using SGA.Domain.Base;
using SGA.Domain.Enums.Users;
using SGA.Domain.ValueObjects.Users;
using System;
using System.Collections.Generic;
using SGA.Domain.Entities.Authorizations;
using SGA.Domain.Exceptions.Users;
namespace SGA.Domain.Entities.Users
{
    public abstract class Usuario : BaseEntity<int>
    {
        public int RolId { get; protected set; }
        
        public Email Email { get; protected set; } = null!;
        public PhoneNumber PhoneNumber { get; protected set; } = null!;
        
        public string FirstName { get; protected set; } = string.Empty;
        public string LastName { get; protected set; } = string.Empty;
        public string PasswordHash { get; protected set; } = string.Empty;
        public string Cedula { get; protected set; } = string.Empty;
        
        public UserType UserType { get; protected set; }
        public UserStatus Status { get; protected set; }
        public bool IsActivo { get; protected set; }
        
        public Rol Rol { get; protected set; }
        public ICollection<Authorization> Authorizations { get; protected set; } = new List<Authorization>();
        
        protected Usuario() { }
        
        protected Usuario(
            string firstName,
            string lastName,
            string email,
            string cedula,
            string phoneNumber,
            int rolId,
            UserType userType,
            string createdBy)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = new Email(email);
            Cedula = cedula;
            PhoneNumber = new PhoneNumber(phoneNumber);
            RolId = rolId;
            UserType = userType;
            Status = UserStatus.Active;
            IsActivo = true;
            
            SetCreationInfo(createdBy);
        }
        
        public void Activate(string modifiedBy)
        {
            if (IsActivo)
                throw new InvalidUserException("User is already active");
            IsActivo = true;
            Status = UserStatus.Active;
            SetModificationInfo(modifiedBy);
        }
        
        public void Deactivate(string modifiedBy)
        {
            if (!IsActivo)
                throw new InvalidUserException("User is already inactive");
            IsActivo = false;
            Status = UserStatus.Inactive;
            SetModificationInfo(modifiedBy);
        }
        
        public void ChangeEmail(string newEmail, string modifiedBy)
        {
            Email = new Email(newEmail);
            SetModificationInfo(modifiedBy);
        }
        
        public string GetFullName() => $"{FirstName} {LastName}";
    }
}


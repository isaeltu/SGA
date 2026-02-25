using SGA.Domain.Enums.Users;
using SGA.Domain.Entities.Trips;
using SGA.Domain.Exceptions.Users;
using System;
using System.Collections.Generic;

namespace SGA.Domain.Entities.Users
{
    public class Driver : Usuario
    {
        public string DriverLicence { get; protected set; } = string.Empty;
        public DateTime LicenceExpirationDate { get; protected set; }
        public bool IsAvailable { get; protected set; }
        
        public ICollection<Trip> Trips { get; protected set; } = new List<Trip>();
        
        protected Driver() { }
        
        public Driver(
            string firstName,
            string lastName,
            string email,
            string cedula,
            string phoneNumber,
            int rolId,
            string driverLicence,
            DateTime licenceExpirationDate,
            string createdBy)
            : base(firstName, lastName, email, cedula, phoneNumber, rolId, UserType.Driver, createdBy)
        {
            DriverLicence = driverLicence;
            LicenceExpirationDate = licenceExpirationDate;
            IsAvailable = true;
        }
        
        public bool IsLicenceValid() => LicenceExpirationDate > DateTime.UtcNow;
        
        public void MarkAsUnavailable(string modifiedBy)
        {
            if (!IsAvailable)
                throw new DriverNotAvailableException("Driver is already unavailable");
            IsAvailable = false;
            SetModificationInfo(modifiedBy);
        }
        
        public void MarkAsAvailable(string modifiedBy)
        {
            if (IsAvailable)
                throw new InvalidUserException("Driver is already available");
            
            if (!IsLicenceValid())
                throw new InvalidUserException("Cannot mark driver as available with expired licence");
            
            IsAvailable = true;
            SetModificationInfo(modifiedBy);
        }
    }
}

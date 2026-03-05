using SGA.Domain.Enums.Users;
using SGA.Domain.Entities.Trips;
using SGA.Domain.Exceptions.Users;
using System;
using System.Collections.Generic;
using SGA.Domain.Base;
using SGA.Domain.Common;
using System.Numerics;

namespace SGA.Domain.Entities.Users
{
    public class Driver : BaseEntity<int>
    {
        public int PersonId { get; set; }
        public string DriverLicence { get; set; }
        public DateTimeOffset LicenceExpirationDate { get; set; }
        public bool IsAvailable { get; set; }
        
        public ICollection<Trip> Trips { get; protected set; } = new List<Trip>();
        
        protected Driver() { }
        
        public Driver(
            int PersonId,
            string driverLicence,
            DateTimeOffset LicenceExpirationDate
            )
        {
            DriverLicence = driverLicence;
            this.LicenceExpirationDate = LicenceExpirationDate;
            IsAvailable = true;
        }
        
        public bool IsLicenceValid() => LicenceExpirationDate > DateTime.UtcNow;
        
        public Result<bool> MarkAsUnavailable(string modifiedBy)
        {
            if (!IsAvailable)
               return Result<bool>.Failure(DomainErrors.Person.Unavailable);
            IsAvailable = false;
            SetModificationInfo(modifiedBy);

            return Result<bool>.Success(IsAvailable);
        }
        
        public Result<bool> MarkAsAvailable(string modifiedBy)
        {
            if (IsAvailable)
                return Result<bool>.Failure(DomainErrors.Person.Available);

            if (!IsLicenceValid())
                return Result<bool>.Failure(DomainErrors.Person.LicenseExpired);
            
            IsAvailable = true;
            SetModificationInfo(modifiedBy);
            return Result<bool>.Success(IsAvailable);
        }
    }
}

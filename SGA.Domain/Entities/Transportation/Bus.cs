using SGA.Domain.Base;
using SGA.Domain.Enums.Transportation;
using SGA.Domain.ValueObjects.Transportation;
using SGA.Domain.Exceptions.Transportation;
using SGA.Domain.Entities.Trips;
using System;
using System.Collections.Generic;

namespace SGA.Domain.Entities.Transportation
{
    public class Bus : BaseEntity<int>
    {
        public int InstitutionId { get; set; }
        public LicensePlate LicensePlate { get; set; } = null!;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public int Capacity { get; set; }
        public int AvailableSeats { get; set; }
        
        public BusStatus Status { get; set; }
        public Users.Institution? Institution { get; set; }
        
        protected Bus() { }
        
        public Bus(
            string licensePlate,
            string model,
            int year,
            int capacity,
            string createdBy)
        {
            LicensePlate = new LicensePlate(licensePlate);
            Model = model;
            Year = year;
            Capacity = capacity;
            AvailableSeats = capacity;
            Status = BusStatus.Available;
            
            SetCreationInfo(createdBy);
        }
        
        public void MarkAsInMaintenance(string modifiedBy)
        {
            if (Status == BusStatus.InMaintenance)
                throw new BusNotAvailableException("Bus is already in maintenance");
            Status = BusStatus.InMaintenance;
            SetModificationInfo(modifiedBy);
        }
        
        public void MarkAsAvailable(string modifiedBy)
        {
            if (Status == BusStatus.Available)
                throw new InvalidOperationException("Bus is already available");
            Status = BusStatus.Available;
            SetModificationInfo(modifiedBy);
        }
    }
}

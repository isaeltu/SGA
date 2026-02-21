using SGA.Domain.Base;
using SGA.Domain.Enums.Transportation;
using SGA.Domain.ValueObjects.Transportation;
using SGA.Domain.Entities.Trips;
using System;
using System.Collections.Generic;

namespace SGA.Domain.Entities.Transportation
{
    public class Bus : BaseEntity<int>
    {
        public LicensePlate LicensePlate { get; protected set; } = null!;
        
        public string Model { get; protected set; } = string.Empty;
        public int Year { get; protected set; }
        public int Capacity { get; protected set; }
        public byte AvailableSeats { get; protected set; }
        
        public BusStatus Status { get; protected set; }
        
        public ICollection<Trip> Trips { get; protected set; } = new List<Trip>();
        
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

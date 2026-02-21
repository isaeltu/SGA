using SGA.Domain.Base;
using SGA.Domain.Enums.Trips;
using SGA.Domain.Entities.Users;
using SGA.Domain.Entities.Transportation;
using SGA.Domain.Entities.Incidents;
using System;
using System.Collections.Generic;

namespace SGA.Domain.Entities.Trips
{
    public class Trip : BaseEntity<int>
    {
        public int RouteId { get; protected set; }
        public int DriverId { get; protected set; }
        public int BusId { get; protected set; }
        
        public DateTime ScheduledDepartureTime { get; protected set; }
        public DateTime? ActualDepartureTime { get; protected set; }
        public DateTime ScheduledArrivalTime { get; protected set; }
        public DateTime? ActualArrivalTime { get; protected set; }
        public int AvailableSeats { get; protected set; }
        
        public TripStatus Status { get; protected set; }
        
        public Route Route { get; protected set; } = null!;
        public Driver Driver { get; protected set; } = null!;
        public Bus Bus { get; protected set; } = null!;
        public ICollection<Reservation> Reservations { get; protected set; } = new List<Reservation>();
        public ICollection<Incident> Incidents { get; protected set; } = new List<Incident>();
        public ICollection<TripStop> TripStops { get; protected set; } = new List<TripStop>();
        
        protected Trip() { }
        
        public Trip(
            int routeId,
            int driverId,
            int busId,
            DateTime scheduledDepartureTime,
            DateTime scheduledArrivalTime,
            int availableSeats,
            string createdBy)
        {
            RouteId = routeId;
            DriverId = driverId;
            BusId = busId;
            ScheduledDepartureTime = scheduledDepartureTime;
            ScheduledArrivalTime = scheduledArrivalTime;
            AvailableSeats = availableSeats;
            Status = TripStatus.Scheduled;
            
            SetCreationInfo(createdBy);
        }
        
        public void Start(string modifiedBy)
        {
            if (Status != TripStatus.Scheduled)
                throw new TripNotAvailableException("Only scheduled trips can be started");
            
            Status = TripStatus.InProgress;
            ActualDepartureTime = DateTime.UtcNow;
            SetModificationInfo(modifiedBy);
        }
        
        public void Complete(string modifiedBy)
        {
            if (Status != TripStatus.InProgress)
                throw new TripNotAvailableException("Only in-progress trips can be completed");
            
            Status = TripStatus.Completed;
            ActualArrivalTime = DateTime.UtcNow;
            SetModificationInfo(modifiedBy);
        }
        
        public void Cancel(string modifiedBy)
        {
            if (Status == TripStatus.Completed)
                throw new TripNotAvailableException("Cannot cancel completed trips");
            
            Status = TripStatus.Cancelled;
            SetModificationInfo(modifiedBy);
        }
    }
}

using SGA.Domain.Base;
using SGA.Domain.Enums.Trips;
using SGA.Domain.Entities.Users;
using SGA.Domain.Entities.Transportation;
using SGA.Domain.Entities.Incidents;
using SGA.Domain.DomainEvents.Trips;
using SGA.Domain.Common;

namespace SGA.Domain.Entities.Trips
{
    public class Trip : BaseEntity<int>
    {
        public int InstitutionId { get; set; }
        public int RouteId { get; set; }
        public int DriverId { get;  set; }
        public int BusId { get;  set; }
        
        public DateTime ScheduledDepartureTime { get; set; }
        public DateTime? ActualDepartureTime { get; set; }
        public DateTime ScheduledArrivalTime { get; set; }
        public DateTime? ActualArrivalTime { get;  set; }
        public int AvailableSeats { get;  set; }
        
        public TripStatus Status { get;  set; }
        
        public Route Route { get;  set; } = null!;
        public Driver Driver { get; set; } = null!;
        public Bus Bus { get; set; } = null!;
        public Institution? Institution { get; set; }
        
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
        
        public Result<TripStatus> Start(string modifiedBy)
        {
            if (Status != TripStatus.Scheduled)
                return Result<TripStatus>.Failure(DomainErrors.Trip.NotScheduled);
            
            Status = TripStatus.InProgress;
            ActualDepartureTime = DateTime.UtcNow;
            SetModificationInfo(modifiedBy);
            AddDomainEvent(new TripStartedDomainEvent(Id));
            return Result<Trip>.Success(Status);
        }
        
        public Result<TripStatus> Complete(string modifiedBy)
        {
            if (Status != TripStatus.InProgress)
                return Result<TripStatus>.Failure(DomainErrors.Trip.NotInProgress);
            
            Status = TripStatus.Completed;
            ActualArrivalTime = DateTime.UtcNow;
            SetModificationInfo(modifiedBy);
            AddDomainEvent(new TripCompletedDomainEvent(Id));
            return Result<Trip>.Success(Status);
        }
        
        public Result<TripStatus> Cancel(string modifiedBy)
        {
            if (Status == TripStatus.Completed)
                return Result<TripStatus>.Failure(DomainErrors.Trip.AlreadyCompleted);
            
            Status = TripStatus.Cancelled;
            SetModificationInfo(modifiedBy);
            AddDomainEvent(new TripCancelledDomainEvent(Id));
            return Result<TripStatus>.Success(Status);

            
        }
    }
}

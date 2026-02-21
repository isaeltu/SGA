using SGA.Domain.Base;
using SGA.Domain.Entities.Transportation;
using System;

namespace SGA.Domain.Entities.Trips
{
    public class TripStop : BaseEntity<int>
    {
        public int TripId { get; protected set; }
        public int StopId { get; protected set; }
        
        public int StopOrder { get; protected set; }
        public DateTime? ArrivalTime { get; protected set; }
        public DateTime? DepartureTime { get; protected set; }
        public int PassengersBoarded { get; protected set; }
        
        public Trip Trip { get; protected set; } = null!;
        public Stop Stop { get; protected set; } = null!;
        
        protected TripStop() { }
        
        public TripStop(
            int tripId,
            int stopId,
            int stopOrder,
            string createdBy)
        {
            TripId = tripId;
            StopId = stopId;
            StopOrder = stopOrder;
            PassengersBoarded = 0;
            
            SetCreationInfo(createdBy);
        }
        
        public void RecordArrival()
        {
            ArrivalTime = DateTime.UtcNow;
        }
        
        public void RecordDeparture()
        {
            DepartureTime = DateTime.UtcNow;
        }
        
        public void IncrementPassengers()
        {
            PassengersBoarded++;
        }
    }
}

using SGA.Domain.Base;
using SGA.Domain.ValueObjects.Transportation;
using System;

namespace SGA.Domain.Entities.Trips
{
    public class GpsLocation : BaseEntity<int>
    {
        public int TripId { get; protected set; }
        
        public Coordinate Location { get; protected set; } = null!;
        
        public DateTime Timestamp { get; protected set; }
        public double? SpeedKmh { get; protected set; }
        public double? Heading { get; protected set; }
        
        public Trip Trip { get; protected set; } = null!;
        
        protected GpsLocation() { }
        
        public GpsLocation(
            int tripId,
            double latitude,
            double longitude,
            double? speedKmh,
            double? heading,
            string createdBy)
        {
            TripId = tripId;
            Location = new Coordinate(latitude, longitude);
            Timestamp = DateTime.UtcNow;
            SpeedKmh = speedKmh;
            Heading = heading;
            
            SetCreationInfo(createdBy);
        }
    }
}

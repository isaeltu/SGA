using SGA.Domain.Base;
using SGA.Domain.Enums.Transportation;
using SGA.Domain.ValueObjects.Transportation;
using System.Collections.Generic;

namespace SGA.Domain.Entities.Transportation
{
    public class Stop : BaseEntity<int>
    {
        public Coordinate Location { get; protected set; } = null!;
        
        public string Name { get; protected set; } = string.Empty;
        public string? Address { get; protected set; }
        
        public StopType Type { get; protected set; }
        
        public ICollection<RouteStop> RouteStops { get; protected set; } = new List<RouteStop>();
        
        protected Stop() { }
        
        public Stop(
            string name,
            string? address,
            double latitude,
            double longitude,
            StopType type,
            string createdBy)
        {
            Name = name;
            Address = address;
            Location = new Coordinate(latitude, longitude);
            Type = type;
            
            SetCreationInfo(createdBy);
        }
    }
}

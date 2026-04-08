using SGA.Domain.Base;
using SGA.Domain.Entities.Trips;
using System;
using System.Collections.Generic;

namespace SGA.Domain.Entities.Transportation
{
    public class Route : BaseEntity<int>
    {
        public int InstitutionId { get; set; }
        public string Name { get; protected set; } = string.Empty;
        public string Origin { get; protected set; } = string.Empty;
        public string Destination { get; protected set; } = string.Empty;
        public decimal DistanceKm { get; protected set; }
        public int EstimatedDurationMinutes { get; protected set; }
        public bool IsActive { get; protected set; }
        public Users.Institution? Institution { get; set; }
        
        public ICollection<RouteStop> RouteStops { get; protected set; } = new List<RouteStop>();
        public ICollection<Trip> Trips { get; protected set; } = new List<Trip>();
        
        protected Route() { }
        
        public Route(
            string name,
            string origin,
            string destination,
            decimal distanceKm,
            int estimatedDurationMinutes,
            string createdBy)
        {
            Name = name;
            Origin = origin;
            Destination = destination;
            DistanceKm = distanceKm;
            EstimatedDurationMinutes = estimatedDurationMinutes;
            IsActive = true;
            
            SetCreationInfo(createdBy);
        }
        
        public void Deactivate(string modifiedBy)
        {
            if (!IsActive)
                throw new InvalidOperationException("Route is already inactive");
            IsActive = false;
            SetModificationInfo(modifiedBy);
        }

        public void Activate(string modifiedBy)
        {
            if (IsActive)
                return;

            IsActive = true;
            SetModificationInfo(modifiedBy);
        }

        public void UpdateDetails(
            string name,
            string origin,
            string destination,
            decimal distanceKm,
            int estimatedDurationMinutes,
            string modifiedBy)
        {
            Name = name;
            Origin = origin;
            Destination = destination;
            DistanceKm = distanceKm;
            EstimatedDurationMinutes = estimatedDurationMinutes;
            SetModificationInfo(modifiedBy);
        }
    }
}

using SGA.Domain.Base;
using SGA.Domain.Enums.Incidents;
using SGA.Domain.Entities.Trips;
using SGA.Domain.Entities.Users;
using SGA.Domain.Exceptions.Incidents;
using System;

namespace SGA.Domain.Entities.Incidents
{
    public class Incident : BaseEntity<int>
    {
        public int TripId { get; set; }
        public int ReportedById { get; set; }
        public int? ResolvedById { get;  set; }
        
        public string Description { get;  set; }
        public DateTime ReportedAt { get; set; }
        public DateTime? ResolvedAt { get;  set; }
        public string? ResolutionNotes { get; set; }
        
        public IncidentType Type { get;  set; }
        public IncidentSeverity Severity { get;  set; }
        public IncidentStatus Status { get;  set; }
        
        public Trip Trip { get; set; }
        public Person ReportedBy { get;  set; }
        public Person? ResolvedBy { get;  set; }
        
        protected Incident() { }
        
        public Incident(
            int tripId,
            int reportedById,
            string description,
            IncidentType type,
            IncidentSeverity severity,
            string createdBy)
        {
            TripId = tripId;
            ReportedById = reportedById;
            Description = description;
            Type = type;
            Severity = severity;
            Status = IncidentStatus.Open;
            ReportedAt = DateTime.UtcNow;
            
            SetCreationInfo(createdBy);
        }
        
        public void Resolve(int resolvedById, string resolutionNotes, string modifiedBy)
        {
            if (Status == IncidentStatus.Resolved)
                throw new InvalidIncidentException("Incident is already resolved");
            
            Status = IncidentStatus.Resolved;
            ResolvedById = resolvedById;
            ResolvedAt = DateTime.UtcNow;
            ResolutionNotes = resolutionNotes;
            
            SetModificationInfo(modifiedBy);
        }
        
        public void Close(string modifiedBy)
        {
            if (Status != IncidentStatus.Resolved)
                throw new InvalidIncidentException("Only resolved incidents can be closed");
            
            Status = IncidentStatus.Closed;
            SetModificationInfo(modifiedBy);
        }
    }
}

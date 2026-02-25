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
        public int TripId { get; protected set; }
        public int ReportedById { get; protected set; }
        public int? ResolvedById { get; protected set; }
        
        public string Description { get; protected set; } = string.Empty;
        public DateTime ReportedAt { get; protected set; }
        public DateTime? ResolvedAt { get; protected set; }
        public string? ResolutionNotes { get; protected set; }
        
        public IncidentType Type { get; protected set; }
        public IncidentSeverity Severity { get; protected set; }
        public IncidentStatus Status { get; protected set; }
        
        public Trip Trip { get; protected set; } = null!;
        public Usuario ReportedBy { get; protected set; } = null!;
        public Usuario? ResolvedBy { get; protected set; }
        
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

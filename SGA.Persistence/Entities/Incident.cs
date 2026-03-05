using System;
using System.Collections.Generic;

namespace SGA.Persistence.Entities;

public partial class Incident
{
    public int Id { get; set; }

    public int TripId { get; set; }

    public int ReportedById { get; set; }

    public int? ResolvedById { get; set; }

    public string Description { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string Severity { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime ReportedAt { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public string? ResolutionNotes { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public virtual Person ReportedBy { get; set; } = null!;

    public virtual Person? ResolvedBy { get; set; }

    public virtual Trip Trip { get; set; } = null!;
}

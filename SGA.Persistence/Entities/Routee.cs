using System;
using System.Collections.Generic;

namespace SGA.Persistence.Entities;

public partial class Routee
{
    public int Id { get; set; }

    public int? InstitutionId { get; set; }

    public string Name { get; set; } = null!;

    public string Origin { get; set; } = null!;

    public string Destination { get; set; } = null!;

    public int EstimatedDurationMinutes { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public virtual Institution? Institution { get; set; }

    public virtual ICollection<RouteStop> RouteStops { get; set; } = new List<RouteStop>();

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}

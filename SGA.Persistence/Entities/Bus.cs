using System;
using System.Collections.Generic;

namespace SGA.Persistence.Entities;

public partial class Bus
{
    public short Id { get; set; }

    public int? InstitutionId { get; set; }

    public string LicensePlate { get; set; } = null!;

    public string? Model { get; set; }

    public int Year { get; set; }

    public int Capacity { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public virtual Institution? Institution { get; set; }

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}

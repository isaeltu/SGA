using System;
using System.Collections.Generic;

namespace SGA.Persistence.Entities;

public partial class College
{
    public byte Id { get; set; }

    public int? InstitutionId { get; set; }

    public int? ModeId { get; set; }

    public string Name { get; set; } = null!;

    public string? TimeEstimated { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public virtual Institution? Institution { get; set; }

    public virtual Mode? Mode { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}

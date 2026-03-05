using System;
using System.Collections.Generic;

namespace SGA.Persistence.Entities;

public partial class Institution
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public virtual ICollection<Bus> Buses { get; set; } = new List<Bus>();

    public virtual ICollection<College> Colleges { get; set; } = new List<College>();

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<Person> People { get; set; } = new List<Person>();

    public virtual ICollection<Routee> Routees { get; set; } = new List<Routee>();

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}

using System;
using System.Collections.Generic;

namespace SGA.Persistence.Entities;

public partial class Student
{
    public int Id { get; set; }

    public int PersonId { get; set; }

    public byte? CollegeId { get; set; }

    public string EnrollmentId { get; set; } = null!;

    public string? Period { get; set; }

    public string? CareerName { get; set; }

    public virtual ICollection<Authorization> Authorizations { get; set; } = new List<Authorization>();

    public virtual ICollection<Boarding> Boardings { get; set; } = new List<Boarding>();

    public virtual College? College { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}

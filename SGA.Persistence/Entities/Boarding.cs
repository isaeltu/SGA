using System;
using System.Collections.Generic;

namespace SGA.Persistence.Entities;

public partial class Boarding
{
    public int Id { get; set; }

    public int ReservationId { get; set; }

    public int TripId { get; set; }

    public int StudentId { get; set; }

    public DateTime BoardingTime { get; set; }

    public string? BoardingStop { get; set; }

    public string BoardingType { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public virtual Reservation Reservation { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;

    public virtual Trip Trip { get; set; } = null!;
}

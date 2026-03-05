using System;
using System.Collections.Generic;

namespace SGA.Persistence.Entities;

public partial class Reservation
{
    public int Id { get; set; }

    public int TripId { get; set; }

    public int StudentId { get; set; }

    public int AuthorizationId { get; set; }

    public int SeatNumber { get; set; }

    public string QrCode { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public virtual Authorization Authorization { get; set; } = null!;

    public virtual ICollection<Boarding> Boardings { get; set; } = new List<Boarding>();

    public virtual Student Student { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual Trip Trip { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace SGA.Persistence.Entities;

public partial class GpsLocation
{
    public long Id { get; set; }

    public int TripId { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public DateTime Timestamp { get; set; }

    public decimal? Speed { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public virtual Trip Trip { get; set; } = null!;
}

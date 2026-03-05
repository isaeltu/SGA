using System;
using System.Collections.Generic;

namespace SGA.Persistence.Entities;

public partial class TripStop
{
    public int Id { get; set; }

    public int TripId { get; set; }

    public int StopId { get; set; }

    public int StopOrder { get; set; }

    public DateTime? ScheduledArrival { get; set; }

    public DateTime? ActualArrival { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public virtual Stop Stop { get; set; } = null!;

    public virtual Trip Trip { get; set; } = null!;
}

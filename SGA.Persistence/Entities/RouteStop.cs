using System;
using System.Collections.Generic;

namespace SGA.Persistence.Entities;

public partial class RouteStop
{
    public int RouteId { get; set; }

    public int StopId { get; set; }

    public int StopOrder { get; set; }

    public int? EstimatedArrivalMinutes { get; set; }

    public virtual Routee Route { get; set; } = null!;

    public virtual Stop Stop { get; set; } = null!;
}

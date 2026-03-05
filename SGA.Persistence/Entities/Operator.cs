using System;
using System.Collections.Generic;

namespace SGA.Persistence.Entities;

public partial class Operator
{
    public int Id { get; set; }

    public int PersonId { get; set; }

    public string? AssignedArea { get; set; }

    public int? ShiftNumber { get; set; }

    public virtual Person Person { get; set; } = null!;
}

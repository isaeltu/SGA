using System;
using System.Collections.Generic;

namespace SGA.Persistence.Entities;

public partial class Administrator
{
    public int Id { get; set; }

    public int PersonId { get; set; }

    public byte AdminLevel { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public virtual Person Person { get; set; } = null!;
}

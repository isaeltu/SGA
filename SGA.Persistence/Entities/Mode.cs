using System;
using System.Collections.Generic;

namespace SGA.Persistence.Entities;

public partial class Mode
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<College> Colleges { get; set; } = new List<College>();
}

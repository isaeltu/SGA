using System;
using System.Collections.Generic;

namespace SGA.Persistence.Entities;

public partial class PersonRole
{
    public int PersonId { get; set; }

    public byte RolId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual Rol Rol { get; set; } = null!;
}

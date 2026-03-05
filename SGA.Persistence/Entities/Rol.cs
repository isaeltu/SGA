using System;
using System.Collections.Generic;

namespace SGA.Persistence.Entities;

public partial class Rol
{
    public byte Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<PersonRole> PersonRoles { get; set; } = new List<PersonRole>();

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}

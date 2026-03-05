using System;
using System.Collections.Generic;

namespace SGA.Persistence.Entities;

public partial class Person
{
    public int Id { get; set; }

    public int InstitutionId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Cedula { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public virtual Administrator? Administrator { get; set; }

    public virtual Driver? Driver { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<Incident> IncidentReportedBies { get; set; } = new List<Incident>();

    public virtual ICollection<Incident> IncidentResolvedBies { get; set; } = new List<Incident>();

    public virtual Institution Institution { get; set; } = null!;

    public virtual Operator? Operator { get; set; }

    public virtual ICollection<PersonRole> PersonRoles { get; set; } = new List<PersonRole>();

    public virtual Student? Student { get; set; }
}

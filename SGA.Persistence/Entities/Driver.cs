using System;
using System.Collections.Generic;

namespace SGA.Persistence.Entities;

public partial class Driver
{
    public int Id { get; set; }

    public int PersonId { get; set; }

    public string DriverLicence { get; set; } = null!;

    public DateTime LicenceExpirationDate { get; set; }

    public bool IsAvailable { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}

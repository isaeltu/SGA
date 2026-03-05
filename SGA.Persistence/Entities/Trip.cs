using System;
using System.Collections.Generic;

namespace SGA.Persistence.Entities;

public partial class Trip
{
    public int Id { get; set; }

    public int? InstitutionId { get; set; }

    public int RouteId { get; set; }

    public int DriverId { get; set; }

    public short BusId { get; set; }

    public DateTime ScheduledDepartureTime { get; set; }

    public DateTime? ActualDepartureTime { get; set; }

    public int AvailableSeats { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public virtual ICollection<Boarding> Boardings { get; set; } = new List<Boarding>();

    public virtual Bus Bus { get; set; } = null!;

    public virtual Driver Driver { get; set; } = null!;

    public virtual ICollection<GpsLocation> GpsLocations { get; set; } = new List<GpsLocation>();

    public virtual ICollection<Incident> Incidents { get; set; } = new List<Incident>();

    public virtual Institution? Institution { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual Routee Route { get; set; } = null!;

    public virtual ICollection<TripStop> TripStops { get; set; } = new List<TripStop>();
}

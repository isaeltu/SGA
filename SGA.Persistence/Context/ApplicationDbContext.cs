using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SGA.Application.Abstractions.Messaging;
using SGA.Domain.DomainEvents;
using SGA.Domain.Entities.Authorizations;
using SGA.Domain.Entities.Incidents;
using SGA.Domain.Entities.Messaging;
using SGA.Domain.Entities.Transportation;
using SGA.Domain.Entities.Trips;
using SGA.Domain.ValueObjects.Users;
using SGA.Domain.Entities.Users;
using SGA.Domain.ValueObjects.Transportation;

namespace SGA.Persistence.Context;

public partial class ApplicationDbContext : DbContext
{
    private readonly IMessageBus? _messageBus;
    private readonly IDomainEventSerializer? _domainEventSerializer;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IMessageBus? messageBus,
        IDomainEventSerializer? domainEventSerializer)
        : base(options)
    {
        _messageBus = messageBus;
        _domainEventSerializer = domainEventSerializer;
    }

    public virtual DbSet<Domain.Entities.Users.Administrator> Administrators { get; set; }

    public virtual DbSet<Authorization> Authorizations { get; set; }

    public virtual DbSet<Boarding> Boardings { get; set; }

    public virtual DbSet<Bus> Buses { get; set; }

    public virtual DbSet<Domain.Entities.Users.College> Colleges { get; set; }

    public virtual DbSet<Domain.Entities.Users.Department> Departments { get; set; }

    public virtual DbSet<Domain.Entities.Users.Driver> Drivers { get; set; }

    public virtual DbSet<Domain.Entities.Users.Employee> Employees { get; set; }

    public virtual DbSet<GpsLocation> GpsLocations { get; set; }

    public virtual DbSet<Incident> Incidents { get; set; }

    public virtual DbSet<Institution> Institutions { get; set; }

    public virtual DbSet<Mode> Modes { get; set; }

    public virtual DbSet<Domain.Entities.Users.Operator> Operators { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Domain.Entities.Users.Person> Persons { get; set;}

    public virtual DbSet<PersonRole> PersonRoles { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<OutboxMessage> OutboxMessages { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RouteStop> RouteStops { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<Stop> Stops { get; set; }

    public virtual DbSet<Domain.Entities.Users.Student> Students { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    public virtual DbSet<TripStop> TripStops { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = CollectDomainEvents();

        if (domainEvents.Count > 0)
        {
            AddOutboxMessages(domainEvents);
        }

        var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        if (domainEvents.Count > 0)
        {
            await PublishOutboxMessagesAsync(domainEvents, cancellationToken).ConfigureAwait(false);
        }

        return result;
    }

    public override int SaveChanges()
    {
        return SaveChangesAsync(CancellationToken.None).GetAwaiter().GetResult();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<SGA.Domain.ValueObjects.Transportation.Coordinate>();

        modelBuilder.Entity<Domain.Entities.Users.Administrator>(entity =>
        {
            entity.HasIndex(e => e.PersonId, "UQ_Administrators_PersonId").IsUnique();

            entity.HasOne(d => d.Person).WithOne(p => p.Administrator)
                .HasForeignKey<Domain.Entities.Users.Administrator>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Administrators_Persons");
        });

        modelBuilder.Entity<Authorization>(entity =>
        {
            entity.Property(e => e.Balance)
                .HasConversion(
                    amount => amount.Amount,
                    value => new SGA.Domain.ValueObjects.Authorizations.Money(value))
                .HasColumnType("decimal(10, 2)");
            entity.ComplexProperty(e => e.ValidityPeriod, period =>
            {
                period.Property(p => p.StartDate)
                    .HasColumnName("StartDate");
                period.Property(p => p.EndDate)
                    .HasColumnName("EndDate");
            });
            entity.ComplexProperty(e => e.ValidityPeriod).IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.DeletedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasConversion<string>()
                .HasDefaultValue(SGA.Domain.Enums.Authorizations.AuthorizationStatus.Active);
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasConversion<string>();

            entity.HasOne(d => d.Student).WithMany(p => p.Authorizations)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Authorizations_Students");
        });

        modelBuilder.Entity<Boarding>(entity =>
        {
            entity.Property(e => e.BoardingStop).HasMaxLength(100);
            entity.Property(e => e.BoardingTime).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.BoardingType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasConversion<string>()
                .HasDefaultValue(SGA.Domain.Enums.Trips.BoardingType.QrCode);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);

            entity.HasOne(d => d.Reservation).WithOne(p => p.Boarding)
                .HasForeignKey<Boarding>(d => d.ReservationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Boardings_Reservations");

            entity.HasOne(d => d.Student).WithMany(p => p.Boardings)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Boardings_Students");

            entity.HasOne(d => d.Trip).WithMany(p => p.Boardings)
                .HasForeignKey(d => d.TripId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Boardings_Trips");
        });

        modelBuilder.Entity<Bus>(entity =>
        {
            entity.ToTable("Bus");

            entity.HasIndex(e => e.InstitutionId, "IX_Bus_InstitutionId");

            entity.HasIndex(e => e.LicensePlate, "UQ_Bus_LicensePlate").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.DeletedBy).HasMaxLength(255);
            entity.Property(e => e.LicensePlate)
                .HasConversion(
                    plate => plate.Value,
                    value => LicensePlate.FromDatabase(value))
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Model).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasConversion<string>()
                .HasDefaultValue(SGA.Domain.Enums.Transportation.BusStatus.Available);

            entity.HasOne(d => d.Institution).WithMany(p => p.Buses)
                .HasForeignKey(d => d.InstitutionId)
                .HasConstraintName("FK_Bus_Institutions");
        });

        modelBuilder.Entity<SGA.Domain.Entities.Users.College>(entity =>
        {
            entity.ToTable("College");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.DeletedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.TimeEstimated)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Institution).WithMany(p => p.Colleges)
                .HasForeignKey(d => d.InstitutionId)
                .HasConstraintName("FK_College_Institutions");

            entity.HasOne(d => d.Mode).WithMany(p => p.Colleges)
                .HasForeignKey(d => d.ModeId)
                .HasConstraintName("FK_College_Mode");
        });

        modelBuilder.Entity<SGA.Domain.Entities.Users.Department>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.DeletedBy).HasMaxLength(255);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Institution).WithMany(p => p.Departments)
                .HasForeignKey(d => d.InstitutionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Departments_Institutions");
        });

        modelBuilder.Entity<SGA.Domain.Entities.Users.Driver>(entity =>
        {
            entity.Ignore(e => e.Trips);

            entity.HasIndex(e => e.DriverLicence, "UQ_Drivers_Licence").IsUnique();

            entity.HasIndex(e => e.PersonId, "UQ_Drivers_PersonId").IsUnique();

            entity.Property(e => e.DriverLicence)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);

            entity.HasOne(d => d.Person).WithOne(p => p.Driver)
                .HasForeignKey<Domain.Entities.Users.Driver>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Drivers_Persons");
        });

        modelBuilder.Entity<SGA.Domain.Entities.Users.Employee>(entity =>
        {
            entity.HasIndex(e => e.EmployeeCode, "UQ_Employees_Code").IsUnique();

            entity.HasIndex(e => e.PersonId, "UQ_Employees_PersonId").IsUnique();

            entity.Property(e => e.EmployeeCode)
                .HasConversion(
                    code => code.Value,
                    value => new EmployeeCode(value))
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Position).HasMaxLength(100);

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_Employees_Departments");

            entity.HasOne(d => d.Person).WithOne(p => p.Employee)
                .HasForeignKey<Domain.Entities.Users.Employee>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employees_Persons");
        });

        modelBuilder.Entity<GpsLocation>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.ComplexProperty(e => e.Location, location =>
            {
                location.Property(p => p.Latitude)
                    .HasColumnName("Latitude")
                    .HasColumnType("decimal(9, 6)");
                location.Property(p => p.Longitude)
                    .HasColumnName("Longitude")
                    .HasColumnType("decimal(9, 6)");
            });
            entity.Property(e => e.SpeedKmh)
                .HasColumnName("Speed")
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Timestamp).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Trip).WithMany(p => p.GpsLocations)
                .HasForeignKey(d => d.TripId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GpsLocations_Trips");
        });

        modelBuilder.Entity<Incident>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.DeletedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ReportedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasConversion<string>()
                .HasDefaultValue(SGA.Domain.Enums.Incidents.IncidentStatus.Open);
            entity.Property(e => e.Type)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasConversion<string>();
            entity.Property(e => e.Severity)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasConversion<string>();

            entity.HasOne(d => d.ReportedBy).WithMany(p => p.IncidentReportedBies)
                .HasForeignKey(d => d.ReportedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Incidents_ReportedBy");

            entity.HasOne(d => d.ResolvedBy).WithMany(p => p.IncidentResolvedBies)
                .HasForeignKey(d => d.ResolvedById)
                .HasConstraintName("FK_Incidents_ResolvedBy");

            entity.HasOne(d => d.Trip).WithMany(p => p.Incidents)
                .HasForeignKey(d => d.TripId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Incidents_Trips");
        });

        modelBuilder.Entity<Institution>(entity =>
        {
            entity.HasIndex(e => e.Code, "UQ_Institutions_Code").IsUnique();

            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.DeletedBy).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<Mode>(entity =>
        {
            entity.ToTable("Mode");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SGA.Domain.Entities.Users.Operator>(entity =>
        {
            entity.HasIndex(e => e.PersonId, "UQ_Operators_PersonId").IsUnique();

            entity.Property(e => e.AssignedArea).HasMaxLength(100);

            entity.HasOne(d => d.Person).WithOne(p => p.Operator)
                .HasForeignKey<Domain.Entities.Users.Operator>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Operators_Persons");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasIndex(e => e.Name, "UQ_Permissions_Name").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SGA.Domain.Entities.Users.Person>(entity =>
        {
            entity.Ignore(e => e.IncidentReportedBies);
            entity.Ignore(e => e.IncidentResolvedBies);

            entity.HasIndex(e => e.Email, "IX_Persons_Email");

            entity.HasIndex(e => e.InstitutionId, "IX_Persons_InstitutionId");

            entity.HasIndex(e => new { e.Cedula, e.InstitutionId }, "UQ_Persons_Cedula_Institution").IsUnique();

            entity.HasIndex(e => new { e.Email, e.InstitutionId }, "UQ_Persons_Email_Institution").IsUnique();

            entity.Property(e => e.Cedula).HasMaxLength(11);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.DeletedBy).HasMaxLength(255);
            entity.Property(e => e.Email)
                .HasConversion(
                    email => email.Value,
                    value => new Email(value))
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired();
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.PasswordHash).HasMaxLength(250);
            entity.Property(e => e.PhoneNumber)
                .HasConversion(
                    phone => phone.Value,
                    value => new PhoneNumber(value))
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Institution).WithMany(p => p.People)
                .HasForeignKey(d => d.InstitutionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Persons_Institutions");
        });

        modelBuilder.Entity<PersonRole>(entity =>
        {
            entity.HasKey(e => new { e.PersonId, e.RoleId });

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Person).WithMany(p => p.PersonRoles)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonRoles_Persons");

            entity.HasOne(d => d.Role).WithMany(p => p.PersonRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonRoles_Rol");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasIndex(e => e.QrCode, "UQ_Reservations_QrCode").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.DeletedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.QrCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasConversion<string>()
                .HasDefaultValue(SGA.Domain.Enums.Trips.ReservationStatus.Confirmed);

            entity.HasOne(d => d.Authorization).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.AuthorizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservations_Authorizations");

            entity.HasOne(d => d.Student).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservations_Students");

            entity.HasOne(d => d.Trip).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.TripId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservations_Trips");
        });

            modelBuilder.Entity<OutboxMessage>(entity =>
            {
                entity.ToTable("OutboxMessages");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MessageType).HasMaxLength(250).IsRequired();
                entity.Property(e => e.Payload).IsRequired();
                entity.Property(e => e.OccurredOnUtc).IsRequired();
                entity.Property(e => e.Error).HasMaxLength(4000);
            });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Rol");

            entity.HasIndex(e => e.Name, "UQ_Rol_Name").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasMany(d => d.Permissions).WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "RolPermission",
                    r => r.HasOne<Permission>().WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_RolPermissions_Permission"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("RolId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_RolPermissions_Rol"),
                    j =>
                    {
                        j.HasKey("RolId", "PermissionId");
                        j.ToTable("RolPermissions");
                    });
        });

        modelBuilder.Entity<RouteStop>(entity =>
        {
            entity.HasKey(e => new { e.RouteId, e.StopId });

            entity.ToTable("RouteStop");

            entity.HasOne(d => d.Route).WithMany(p => p.RouteStops)
                .HasForeignKey(d => d.RouteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RouteStop_Route");

            entity.HasOne(d => d.Stop).WithMany(p => p.RouteStops)
                .HasForeignKey(d => d.StopId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RouteStop_Stop");
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Route");

            entity.ToTable("Routee");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.DeletedBy).HasMaxLength(255);
            entity.Property(e => e.Destination).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Origin).HasMaxLength(100);

            entity.HasOne(d => d.Institution).WithMany(p => p.Routes)
                .HasForeignKey(d => d.InstitutionId)
                .HasConstraintName("FK_Route_Institutions");
        });

        modelBuilder.Entity<Stop>(entity =>
        {
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.DeletedBy).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.ComplexProperty(e => e.Location, location =>
            {
                location.Property(p => p.Latitude)
                    .HasColumnName("Latitude")
                    .HasColumnType("decimal(9, 6)");
                location.Property(p => p.Longitude)
                    .HasColumnName("Longitude")
                    .HasColumnType("decimal(9, 6)");
            });
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Domain.Entities.Users.Student>(entity =>
        {
            entity.HasIndex(e => e.PersonId, "UQ_Students_PersonId").IsUnique();

            entity.Property(e => e.CareerName).HasMaxLength(100);
            entity.Property(e => e.EnrollmentId)
                .HasConversion(
                    enrollment => enrollment.Value,
                    value => new EnrollmentId(value))
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.College).WithMany(p => p.Students)
                .HasForeignKey(d => d.CollegeId)
                .HasConstraintName("FK_Students_College");

            entity.HasOne(d => d.Person).WithOne(p => p.Student)
                .HasForeignKey<Domain.Entities.Users.Student>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Students_Persons");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.Property(e => e.Amount)
                .HasConversion(
                    amount => amount.Amount,
                    value => new SGA.Domain.ValueObjects.Authorizations.Money(value))
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.Type)
                .HasColumnName("TransactionType")
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasConversion(
                    value => value.ToString(),
                    value => (SGA.Domain.Enums.Authorizations.TransactionType)Enum.Parse(typeof(SGA.Domain.Enums.Authorizations.TransactionType), value));

            entity.HasOne(d => d.Authorization).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.AuthorizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transactions_Authorizations");

        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.DeletedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasConversion<string>()
                .HasDefaultValue(SGA.Domain.Enums.Trips.TripStatus.Scheduled);

            entity.HasOne(d => d.Bus).WithMany(p => p.Trips)
                .HasForeignKey(d => d.BusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Trips_Bus");

            entity.HasOne(d => d.Driver).WithMany(p => p.Trips)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Trips_Driver");

            entity.HasOne(d => d.Institution).WithMany(p => p.Trips)
                .HasForeignKey(d => d.InstitutionId)
                .HasConstraintName("FK_Trips_Institutions");

            entity.HasOne(d => d.Route).WithMany(p => p.Trips)
                .HasForeignKey(d => d.RouteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Trips_Route");
        });

        modelBuilder.Entity<TripStop>(entity =>
        {
            entity.ToTable("TripStop");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);

            entity.HasOne(d => d.Stop).WithMany(p => p.TripStops)
                .HasForeignKey(d => d.StopId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TripStop_Stop");

            entity.HasOne(d => d.Trip).WithMany(p => p.TripStops)
                .HasForeignKey(d => d.TripId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TripStop_Trip");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    private List<IDomainEvent> CollectDomainEvents()
    {
        var domainEvents = ChangeTracker
            .Entries<IHasDomainEvents>()
            .Select(entry => entry.Entity)
            .SelectMany(entity => entity.DomainEvents)
            .ToList();

        foreach (var entity in ChangeTracker.Entries<IHasDomainEvents>().Select(x => x.Entity))
        {
            entity.ClearDomainEvents();
        }

        return domainEvents;
    }

    private void AddOutboxMessages(IEnumerable<IDomainEvent> domainEvents)
    {
        if (_domainEventSerializer is null)
        {
            return;
        }

        var outbox = domainEvents.Select(domainEvent =>
        {
            var message = _domainEventSerializer.Serialize(domainEvent);
            return new OutboxMessage
            {
                Id = Guid.NewGuid(),
                MessageType = message.MessageType,
                Payload = message.Payload,
                OccurredOnUtc = message.OccurredOnUtc
            };
        }).ToList();

        OutboxMessages.AddRange(outbox);
    }

    private async Task PublishOutboxMessagesAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken)
    {
        if (_messageBus is null || _domainEventSerializer is null)
        {
            return;
        }

        foreach (var domainEvent in domainEvents)
        {
            var message = _domainEventSerializer.Serialize(domainEvent);
            await _messageBus.PublishAsync(message, cancellationToken).ConfigureAwait(false);
        }
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

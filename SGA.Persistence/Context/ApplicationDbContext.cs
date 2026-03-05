using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SGA.Persistence.Entities;

namespace SGA.Persistence.Context;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrator> Administrators { get; set; }

    public virtual DbSet<Authorization> Authorizations { get; set; }

    public virtual DbSet<Boarding> Boardings { get; set; }

    public virtual DbSet<Bus> Buses { get; set; }

    public virtual DbSet<College> Colleges { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Driver> Drivers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<GpsLocation> GpsLocations { get; set; }

    public virtual DbSet<Incident> Incidents { get; set; }

    public virtual DbSet<Institution> Institutions { get; set; }

    public virtual DbSet<Mode> Modes { get; set; }

    public virtual DbSet<Operator> Operators { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Person> Persons { get; set; }

    public virtual DbSet<PersonRole> PersonRoles { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<RouteStop> RouteStops { get; set; }

    public virtual DbSet<Routee> Routees { get; set; }

    public virtual DbSet<Stop> Stops { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    public virtual DbSet<TripStop> TripStops { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrator>(entity =>
        {
            entity.HasIndex(e => e.PersonId, "UQ_Administrators_PersonId").IsUnique();

            entity.HasOne(d => d.Person).WithOne(p => p.Administrator)
                .HasForeignKey<Administrator>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Administrators_Persons");
        });

        modelBuilder.Entity<Authorization>(entity =>
        {
            entity.Property(e => e.Balance).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.DeletedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Active");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false);

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
                .HasDefaultValue("QrCode");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);

            entity.HasOne(d => d.Reservation).WithMany(p => p.Boardings)
                .HasForeignKey(d => d.ReservationId)
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
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Model).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValue("Available");

            entity.HasOne(d => d.Institution).WithMany(p => p.Buses)
                .HasForeignKey(d => d.InstitutionId)
                .HasConstraintName("FK_Bus_Institutions");
        });

        modelBuilder.Entity<College>(entity =>
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

        modelBuilder.Entity<Department>(entity =>
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

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasIndex(e => e.DriverLicence, "UQ_Drivers_Licence").IsUnique();

            entity.HasIndex(e => e.PersonId, "UQ_Drivers_PersonId").IsUnique();

            entity.Property(e => e.DriverLicence)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);

            entity.HasOne(d => d.Person).WithOne(p => p.Driver)
                .HasForeignKey<Driver>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Drivers_Persons");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasIndex(e => e.EmployeeCode, "UQ_Employees_Code").IsUnique();

            entity.HasIndex(e => e.PersonId, "UQ_Employees_PersonId").IsUnique();

            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Position).HasMaxLength(100);

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_Employees_Departments");

            entity.HasOne(d => d.Person).WithOne(p => p.Employee)
                .HasForeignKey<Employee>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employees_Persons");
        });

        modelBuilder.Entity<GpsLocation>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Speed).HasColumnType("decimal(5, 2)");
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
            entity.Property(e => e.Severity)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Open");
            entity.Property(e => e.Type)
                .HasMaxLength(30)
                .IsUnicode(false);

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

        modelBuilder.Entity<Operator>(entity =>
        {
            entity.HasIndex(e => e.PersonId, "UQ_Operators_PersonId").IsUnique();

            entity.Property(e => e.AssignedArea).HasMaxLength(100);

            entity.HasOne(d => d.Person).WithOne(p => p.Operator)
                .HasForeignKey<Operator>(d => d.PersonId)
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

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasIndex(e => e.Email, "IX_Persons_Email");

            entity.HasIndex(e => e.InstitutionId, "IX_Persons_InstitutionId");

            entity.HasIndex(e => new { e.Cedula, e.InstitutionId }, "UQ_Persons_Cedula_Institution").IsUnique();

            entity.HasIndex(e => new { e.Email, e.InstitutionId }, "UQ_Persons_Email_Institution").IsUnique();

            entity.Property(e => e.Cedula).HasMaxLength(11);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.DeletedBy).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.PasswordHash).HasMaxLength(250);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.Institution).WithMany(p => p.People)
                .HasForeignKey(d => d.InstitutionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Persons_Institutions");
        });

        modelBuilder.Entity<PersonRole>(entity =>
        {
            entity.HasKey(e => new { e.PersonId, e.RolId });

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Person).WithMany(p => p.PersonRoles)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonRoles_Persons");

            entity.HasOne(d => d.Rol).WithMany(p => p.PersonRoles)
                .HasForeignKey(d => d.RolId)
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
                .HasDefaultValue("Confirmed");

            entity.HasOne(d => d.Authorization).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.AuthorizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservations_Authorizations");

            entity.HasOne(d => d.Student).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservations_Students");

            entity.HasOne(d => d.Trip).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.TripId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservations_Trips");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.ToTable("Rol");

            entity.HasIndex(e => e.Name, "UQ_Rol_Name").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasMany(d => d.Permissions).WithMany(p => p.Rols)
                .UsingEntity<Dictionary<string, object>>(
                    "RolPermission",
                    r => r.HasOne<Permission>().WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_RolPermissions_Permission"),
                    l => l.HasOne<Rol>().WithMany()
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

        modelBuilder.Entity<Routee>(entity =>
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

            entity.HasOne(d => d.Institution).WithMany(p => p.Routees)
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
            entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasIndex(e => e.PersonId, "UQ_Students_PersonId").IsUnique();

            entity.Property(e => e.CareerName).HasMaxLength(100);
            entity.Property(e => e.EnrollmentId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.College).WithMany(p => p.Students)
                .HasForeignKey(d => d.CollegeId)
                .HasConstraintName("FK_Students_College");

            entity.HasOne(d => d.Person).WithOne(p => p.Student)
                .HasForeignKey<Student>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Students_Persons");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BalanceAfter).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.TransactionType)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Authorization).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.AuthorizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transactions_Authorizations");

            entity.HasOne(d => d.Reservation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.ReservationId)
                .HasConstraintName("FK_Transactions_Reservations");
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
                .HasDefaultValue("Scheduled");

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

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

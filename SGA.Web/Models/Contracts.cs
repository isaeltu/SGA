namespace SGA.Web.Models;

public sealed class ApiSettings
{
    public string BaseUrl { get; set; } = "https://localhost:7200";
}

public sealed record CreateInstitutionRequest(string Code, string Name, string CreatedBy);
public sealed record InstitutionResponse(int Id, string Code, string Name, bool IsActive);

public sealed record CreateBusRequest(int InstitutionId, string LicensePlate, string Model, int Year, int Capacity, string CreatedBy);
public sealed record BusResponse(int Id, int InstitutionId, string LicensePlate, string Model, int Year, int Capacity, int AvailableSeats, string Status);

public sealed record CreateReservationRequest(int TripId, int PersonId, int AuthorizationId, string CreatedBy);
public sealed record CreateGuestReservationRequest(
    int InstitutionId,
    int TripId,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string CreatedBy);
public sealed record BoardReservationRequest(int ReservationId, string ModifiedBy);
public sealed record ReservationResponse(int Id, int TripId, int PersonId, int AuthorizationId, int QueueNumber, string QrCode, string Status, DateTime CreatedAt);

public sealed record CreateRoleRequest(string Name, string? Description, string CreatedBy);
public sealed record CreatePermissionRequest(string Name, string? Description, string CreatedBy);

public sealed record CreateTripRequest(
    int RouteId,
    int DriverId,
    int BusId,
    int? InstitutionId,
    DateTime ScheduledDepartureTime,
    DateTime ScheduledArrivalTime,
    int? AvailableSeats,
    string CreatedBy);

public sealed record StartTripRequest(int TripId, string ModifiedBy);
public sealed record CompleteTripRequest(int TripId, string ModifiedBy);
public sealed record CancelTripRequest(int TripId, string ModifiedBy);

public sealed record TripResponse(
    int Id,
    int InstitutionId,
    int RouteId,
    int DriverId,
    int BusId,
    string Status,
    DateTime ScheduledDepartureTime,
    DateTime ScheduledArrivalTime,
    DateTime? ActualDepartureTime,
    DateTime? ActualArrivalTime,
    int AvailableSeats);

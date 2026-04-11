namespace SGA.Web.Models;

public sealed class ApiSettings
{
    public string BaseUrl { get; set; } = "https://localhost:7200";
}

public sealed record CreateInstitutionRequest(string Code, string Name, string CreatedBy);
public sealed record UpdateInstitutionRequest(int InstitutionId, string Code, string Name, bool IsActive, string ModifiedBy);
public sealed record InstitutionResponse(int Id, string Code, string Name, bool IsActive);

public sealed record CreateBusRequest(int InstitutionId, string LicensePlate, string Model, int Year, int Capacity, string CreatedBy);
public sealed record UpdateBusRequest(int BusId, int InstitutionId, string LicensePlate, string Model, int Year, int Capacity, string ModifiedBy);
public sealed record BusResponse(int Id, int InstitutionId, string LicensePlate, string Model, int Year, int Capacity, int AvailableSeats, string Status);

public sealed record CreateRouteRequest(
    int InstitutionId,
    string Name,
    string Origin,
    string Destination,
    decimal DistanceKm,
    int EstimatedDurationMinutes,
    string CreatedBy);
public sealed record RouteResponse(
    int Id,
    int InstitutionId,
    string Name,
    string Origin,
    string Destination,
    decimal DistanceKm,
    int EstimatedDurationMinutes,
    bool IsActive);
public sealed record UpdateRouteRequest(
    int RouteId,
    int InstitutionId,
    string Name,
    string Origin,
    string Destination,
    decimal DistanceKm,
    int EstimatedDurationMinutes,
    bool IsActive,
    string ModifiedBy);

public sealed record CreateReservationRequest(int TripId, int PersonId, int AuthorizationId, string CreatedBy);
public sealed record CreateGuestReservationRequest(
    int InstitutionId,
    int TripId,
    string? FirstName,
    string? LastName,
    string? Email,
    string? PhoneNumber,
    string CreatedBy);
public sealed record BoardReservationRequest(int ReservationId, string ModifiedBy);
public sealed record ReservationResponse(int Id, int TripId, int PersonId, int AuthorizationId, int QueueNumber, string QrCode, string Status, DateTime CreatedAt);

public sealed record CreateRoleRequest(string Name, string? Description, string CreatedBy);
public sealed record CreatePermissionRequest(string Name, string? Description, string CreatedBy);

public sealed record PortalLoginRequest(string Email);
public sealed record OtpRequest(string Email);
public sealed record OtpRequestResponse(string Message, string? DevelopmentCode);
public sealed record OtpVerifyRequest(string Email, string Code);
public sealed record MasterOtpRequest(string Email);
public sealed record MasterOtpRequestResponse(string Message, string? DevelopmentCode);
public sealed record MasterOtpVerifyRequest(string Email, string Code);
public sealed record PortalLoginResponse(
    int PersonId,
    int InstitutionId,
    string FirstName,
    string LastName,
    string Email,
    bool IsAdmin,
    bool IsOperator,
    bool IsDriver,
    bool IsClientOrStudent,
    bool IsMasterAdmin);

public sealed record CreatePersonRequest(
    int InstitutionId,
    int RoleId,
    string Email,
    string PhoneNumber,
    string FirstName,
    string LastName,
    string Cedula,
    string CreatedBy);

public sealed record CreateStudentUserRequest(int PersonId, int CollegeId, string EnrollmentId, string Period, string CareerName, string CreatedBy);
public sealed record CreateOperatorUserRequest(int PersonId, string AssignedArea, int ShiftNumber, string CreatedBy);
public sealed record CreateDriverUserRequest(int PersonId, string DriverLicense, DateTimeOffset LicenseExpirationDate, string CreatedBy);
public sealed record DriverLookupResponse(
    int DriverId,
    int PersonId,
    int InstitutionId,
    string FullName,
    string DriverLicense,
    bool IsAvailable);
public sealed record CreateEmployeeUserRequest(int PersonId, int DepartmentId, string EmployeeCode, string Position, DateTimeOffset HireDate, string CreatedBy);
public sealed record CreateAdministratorUserRequest(int PersonId, int AdminLevel, string CreatedBy);

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
    string? RouteName,
    int DriverId,
    int BusId,
    string Status,
    DateTime ScheduledDepartureTime,
    DateTime ScheduledArrivalTime,
    DateTime? ActualDepartureTime,
    DateTime? ActualArrivalTime,
    int AvailableSeats);

public sealed record AdminErrorLogResponse(
    string ErrorId,
    DateTimeOffset Timestamp,
    int Status,
    string ErrorCode,
    string Message,
    string? StackTrace,
    string Path,
    string Method,
    string? User,
    string? Ip,
    string TraceIdentifier,
    IDictionary<string, string[]>? ValidationErrors);

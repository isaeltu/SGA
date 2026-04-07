namespace SGA.Application.DTOs.Transportation
{
    public sealed record BusDto(
        int Id,
        int InstitutionId,
        string LicensePlate,
        string Model,
        int Year,
        int Capacity,
        int AvailableSeats,
        string Status);
}

namespace SGA.Application.DTOs.Users
{
    public sealed record DriverLookupDto(
        int DriverId,
        int PersonId,
        int InstitutionId,
        string FullName,
        string DriverLicense,
        bool IsAvailable);
}

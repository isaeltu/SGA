namespace SGA.Application.DTOs.Users
{
    public sealed record PersonDto(
        int Id,
        int InstitutionId,
        string Email,
        string FirstName,
        string LastName,
        string? Cedula,
        bool IsActive);
}

namespace SGA.Application.DTOs.Users
{
    public sealed record InstitutionDto(
        int Id,
        string Code,
        string Name,
        bool IsActive);
}

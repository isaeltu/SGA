namespace SGA.Application.DTOs.Users
{
    public sealed record StudentDto(
        int Id,
        int PersonId,
        string EnrollmentId,
        string? CareerName,
        string? Period,
        PersonDto Person);
}

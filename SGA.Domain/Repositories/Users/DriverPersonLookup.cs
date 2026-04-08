namespace SGA.Domain.Repositories.Users
{
    public sealed record DriverPersonLookup(
        int PersonId,
        int InstitutionId,
        string FullName,
        bool IsDeleted);
}

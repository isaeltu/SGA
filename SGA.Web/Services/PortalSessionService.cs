namespace SGA.Web.Services;

public enum PortalRole
{
    None = 0,
    Admin = 1,
    Operator = 2,
    Driver = 3,
    ClientStudent = 4
}

public sealed class PortalSessionService
{
    public event Action? Changed;

    public int PersonId { get; private set; }
    public int InstitutionId { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public bool IsAdmin { get; private set; }
    public bool IsOperator { get; private set; }
    public bool IsDriver { get; private set; }
    public bool IsClientOrStudent { get; private set; }
    public bool IsMasterAdmin { get; private set; }

    public bool IsLoggedIn => PersonId > 0 || IsMasterAdmin;

    public void SignIn(SGA.Web.Models.PortalLoginResponse profile)
    {
        PersonId = profile.PersonId;
        InstitutionId = profile.InstitutionId;
        FullName = $"{profile.FirstName} {profile.LastName}".Trim();
        Email = profile.Email;
        IsAdmin = profile.IsAdmin;
        IsOperator = profile.IsOperator;
        IsDriver = profile.IsDriver;
        IsClientOrStudent = profile.IsClientOrStudent;
        IsMasterAdmin = profile.IsMasterAdmin;
        Changed?.Invoke();
    }

    public void SignOut()
    {
        PersonId = 0;
        InstitutionId = 0;
        FullName = string.Empty;
        Email = string.Empty;
        IsAdmin = false;
        IsOperator = false;
        IsDriver = false;
        IsClientOrStudent = false;
        IsMasterAdmin = false;
        Changed?.Invoke();
    }

    public PortalSessionSnapshot Export()
        => new(
            PersonId,
            InstitutionId,
            FullName,
            Email,
            IsAdmin,
            IsOperator,
            IsDriver,
            IsClientOrStudent,
            IsMasterAdmin);

    public void Restore(PortalSessionSnapshot snapshot)
    {
        PersonId = snapshot.PersonId;
        InstitutionId = snapshot.InstitutionId;
        FullName = snapshot.FullName;
        Email = snapshot.Email;
        IsAdmin = snapshot.IsAdmin;
        IsOperator = snapshot.IsOperator;
        IsDriver = snapshot.IsDriver;
        IsClientOrStudent = snapshot.IsClientOrStudent;
        IsMasterAdmin = snapshot.IsMasterAdmin;
        Changed?.Invoke();
    }

    public bool CanAccess(PortalRole role)
    {
        if (!IsLoggedIn)
        {
            return false;
        }

        return role switch
        {
            PortalRole.Admin => IsAdmin || IsMasterAdmin,
            PortalRole.Operator => IsOperator,
            PortalRole.Driver => IsDriver,
            PortalRole.ClientStudent => IsClientOrStudent,
            _ => false
        };
    }

    public sealed record PortalSessionSnapshot(
        int PersonId,
        int InstitutionId,
        string FullName,
        string Email,
        bool IsAdmin,
        bool IsOperator,
        bool IsDriver,
        bool IsClientOrStudent,
        bool IsMasterAdmin);
}
using SGA.Domain.Common;
using SGA.Domain.Entities.Users;

namespace SGA.Domain.Services.Interfaces.Users
{
    public interface IUserDomainService
    {
        Result ActivateUser(Usuario usuario, string modifiedBy);
        Result DeactivateUser(Usuario usuario, string modifiedBy);
        Result MarkDriverAvailable(Driver driver, string modifiedBy);
        Result MarkDriverUnavailable(Driver driver, string modifiedBy);
    }
}

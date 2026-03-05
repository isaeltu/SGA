using SGA.Domain.Common;
using SGA.Domain.Entities.Users;

namespace SGA.Domain.Services.Interfaces.Users
{
    public interface IUserDomainService
    {
        Task<Person> ActivateUserAsync(Person person, string modifiedBy);
        Task<Person> DeactivateUserAsync(Person person, string modifiedBy);
        Task<Driver> MarkDriverAvailableAsync(Driver driver, string modifiedBy);
        Task<Driver> MarkDriverUnavailableAsync(Driver driver, string modifiedBy);
    }
}

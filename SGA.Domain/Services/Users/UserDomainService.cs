using SGA.Domain.Entities.Users;
using SGA.Domain.Services.Interfaces.Users;

namespace SGA.Domain.Services.Users
{
    public class UserDomainService : IUserDomainService
    {
        public Task<Person> ActivateUserAsync(Person person, string modifiedBy)
        {
            person.SetModificationInfo(modifiedBy);
            return Task.FromResult(person);
        }

        public Task<Person> DeactivateUserAsync(Person person, string modifiedBy)
        {
            person.SetModificationInfo(modifiedBy);
            return Task.FromResult(person);
        }

        public Task<Driver> MarkDriverAvailableAsync(Driver driver, string modifiedBy)
        {
            driver.IsAvailable = true;
            driver.SetModificationInfo(modifiedBy);
            return Task.FromResult(driver);
        }

        public Task<Driver> MarkDriverUnavailableAsync(Driver driver, string modifiedBy)
        {
            driver.IsAvailable = false;
            driver.SetModificationInfo(modifiedBy);
            return Task.FromResult(driver);
        }
    }
}

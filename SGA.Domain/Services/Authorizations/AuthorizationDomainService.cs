using SGA.Domain.Entities.Authorizations;
using SGA.Domain.Services.Interfaces.Authorizations;

namespace SGA.Domain.Services.Authorizations
{
    public class AuthorizationDomainService : IAuthorizationDomainService
    {
        public Task AddBalanceAsync(Authorization authorization, decimal amount, string modifiedBy)
        {
            authorization.AddBalance(amount, modifiedBy);
            return Task.CompletedTask;
        }

        public Task DeductBalanceAsync(Authorization authorization, decimal amount, string modifiedBy)
        {
            authorization.DeductBalance(amount, modifiedBy);
            return Task.CompletedTask;
        }

        public Task<bool> ValidateAsync(Authorization authorization)
        {
            return Task.FromResult(!authorization.IsDeleted && authorization.IsValid());
        }
    }
}

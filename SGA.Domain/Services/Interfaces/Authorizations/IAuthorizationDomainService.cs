using SGA.Domain.Common;
using SGA.Domain.Entities.Authorizations;

namespace SGA.Domain.Services.Interfaces.Authorizations
{
    public interface IAuthorizationDomainService
    {
         Task AddBalanceAsync(Authorization authorization, decimal amount, string modifiedBy);
         Task DeductBalanceAsync(Authorization authorization, decimal amount, string modifiedBy);
         Task<bool> ValidateAsync(Authorization authorization);
    }
}

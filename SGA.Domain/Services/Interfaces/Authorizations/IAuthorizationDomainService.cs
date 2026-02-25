using SGA.Domain.Common;
using SGA.Domain.Entities.Authorizations;

namespace SGA.Domain.Services.Interfaces.Authorizations
{
    public interface IAuthorizationDomainService
    {
        Result AddBalance(Authorization authorization, decimal amount, string modifiedBy);
        Result DeductBalance(Authorization authorization, decimal amount, string modifiedBy);
        Result<bool> Validate(Authorization authorization);
    }
}

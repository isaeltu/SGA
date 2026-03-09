using MediatR;

namespace SGA.Application.Commands
{
    public sealed record CreateDriverCommand(
        int PersonId,
        string DriverLicense,
        DateTimeOffset LicenseExpirationDate,
        string CreatedBy) : IRequest<int>;
}

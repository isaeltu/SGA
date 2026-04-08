using MediatR;
using SGA.Application.DTOs.Users;

namespace SGA.Application.Queries
{
    public sealed record GetDriversQuery(int? InstitutionId = null, bool OnlyAvailable = true)
        : IRequest<IReadOnlyCollection<DriverLookupDto>>;
}

using MediatR;
using SGA.Application.DTOs.Users;

namespace SGA.Application.Queries
{
    public sealed record GetInstitutionsQuery : IRequest<IReadOnlyCollection<InstitutionDto>>;
}
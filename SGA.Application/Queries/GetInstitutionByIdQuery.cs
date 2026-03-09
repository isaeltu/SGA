using MediatR;
using SGA.Application.DTOs.Users;

namespace SGA.Application.Queries
{
    public sealed record GetInstitutionByIdQuery(int InstitutionId) : IRequest<InstitutionDto?>;
}

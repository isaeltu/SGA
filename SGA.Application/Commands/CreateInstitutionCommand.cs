using MediatR;
using SGA.Application.Abstractions.MultiTenancy;

namespace SGA.Application.Commands
{
    public sealed record CreateInstitutionCommand(
        string Code,
        string Name,
        string CreatedBy) : IRequest<int>;
}
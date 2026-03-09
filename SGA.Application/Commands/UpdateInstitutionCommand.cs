using MediatR;

namespace SGA.Application.Commands
{
    public sealed record UpdateInstitutionCommand(
        int InstitutionId,
        string Code,
        string Name,
        bool IsActive,
        string ModifiedBy) : IRequest;
}

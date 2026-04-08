using MediatR;

namespace SGA.Application.Commands
{
    public sealed record DeleteInstitutionCommand(int InstitutionId) : IRequest;
}

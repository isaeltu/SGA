using MediatR;

namespace SGA.Application.Commands
{
    public sealed record CreateBusCommand(
        int InstitutionId,
        string LicensePlate,
        string Model,
        int Year,
        int Capacity,
        string CreatedBy) : IRequest<int>;
}

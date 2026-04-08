using MediatR;

namespace SGA.Application.Commands
{
    public sealed record UpdateBusCommand(
        int BusId,
        int InstitutionId,
        string LicensePlate,
        string Model,
        int Year,
        int Capacity,
        string ModifiedBy) : IRequest;
}

using MediatR;

namespace SGA.Application.Commands
{
    public sealed record CreateTripCommand(
        int RouteId,
        int DriverId,
        int BusId,
        int? InstitutionId,
        DateTime ScheduledDepartureTime,
        DateTime ScheduledArrivalTime,
        int? AvailableSeats,
        string CreatedBy) : IRequest<int>;
}

using MediatR;
using SGA.Application.DTOs.Trips;

namespace SGA.Application.Queries
{
    public sealed record GetReservationByIdQuery(int ReservationId) : IRequest<ReservationDto?>;
}

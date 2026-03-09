using MediatR;
using SGA.Application.DTOs.Transportation;

namespace SGA.Application.Queries
{
    public sealed record GetTripByIdQuery(int TripId) : IRequest<TripDto?>;
}

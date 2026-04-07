using MediatR;
using SGA.Application.DTOs.Transportation;

namespace SGA.Application.Queries
{
    public sealed record GetTripsQuery(int? InstitutionId, bool OnlyBookable) : IRequest<IReadOnlyCollection<TripDto>>;
}
using MediatR;

namespace SGA.Application.Commands
{
    public sealed record CreateGuestReservationCommand(
        int InstitutionId,
        int TripId,
        string? FirstName,
        string? LastName,
        string? Email,
        string? PhoneNumber,
        string CreatedBy) : IRequest<int>;
}
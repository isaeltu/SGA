using SGA.Domain.Common;
using SGA.Domain.Entities.Trips;

namespace SGA.Domain.Services.Interfaces.Trips
{
    public interface ITripDomainService
    {
        Result StartTrip(Trip trip, string modifiedBy);
        Result CompleteTrip(Trip trip, string modifiedBy);
        Result CancelTrip(Trip trip, string modifiedBy);
        Result BoardReservation(Reservation reservation, string modifiedBy);
        Result CancelReservation(Reservation reservation, string modifiedBy);
    }
}

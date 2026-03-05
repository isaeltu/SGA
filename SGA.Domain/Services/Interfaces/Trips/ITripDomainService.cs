using SGA.Domain.Common;
using SGA.Domain.Entities.Transportation;
using SGA.Domain.Entities.Trips;
using SGA.Domain.Entities.Users;

namespace SGA.Domain.Services.Interfaces.Trips
{
    public interface ITripDomainService
    {
        Result StartTrip(Trip trip, Bus bus, Driver driver, string modifiedBy);
        Result CompleteTrip(Trip trip, Bus bus, Driver driver , string modifiedBy);
        Result CancelTrip(Trip trip, Bus bus, Driver driver, string modifiedBy);
        Result BoardReservation(Reservation reservation, string modifiedBy);
        Result CancelReservation(Reservation reservation, string modifiedBy);
        Task<bool> PastTrip(Reservation reservation, Trip trip);
    }
}

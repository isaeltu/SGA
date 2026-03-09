using SGA.Domain.Common;
using SGA.Domain.Entities.Transportation;
using SGA.Domain.Entities.Trips;
using SGA.Domain.Entities.Users;
using SGA.Domain.Enums.Trips;
using SGA.Domain.Services.Interfaces.Trips;

namespace SGA.Domain.Services.Trips
{
    public class TripDomainService : ITripDomainService
    {
        public Result StartTrip(Trip trip, Bus bus, Driver driver, string modifiedBy)
        {
            if (!driver.IsAvailable || bus.AvailableSeats <= 0)
            {
                return Result.Failure(new Error("Trip.Start.InvalidState", "No se puede iniciar el viaje por estado de recursos."));
            }

            trip.Status = TripStatus.InProgress;
            trip.ActualDepartureTime = DateTime.UtcNow;
            trip.SetModificationInfo(modifiedBy);
            return Result.Success();
        }

        public Result CompleteTrip(Trip trip, Bus bus, Driver driver, string modifiedBy)
        {
            if (trip.Status != TripStatus.InProgress)
            {
                return Result.Failure(new Error("Trip.Complete.InvalidState", "El viaje no esta en progreso."));
            }

            trip.Status = TripStatus.Completed;
            trip.ActualArrivalTime = DateTime.UtcNow;
            trip.SetModificationInfo(modifiedBy);
            return Result.Success();
        }

        public Result CancelTrip(Trip trip, Bus bus, Driver driver, string modifiedBy)
        {
            if (trip.Status == TripStatus.Completed)
            {
                return Result.Failure(new Error("Trip.Cancel.InvalidState", "Un viaje completado no puede cancelarse."));
            }

            trip.Status = TripStatus.Cancelled;
            trip.SetModificationInfo(modifiedBy);
            return Result.Success();
        }

        public Result BoardReservation(Reservation reservation, string modifiedBy)
        {
            reservation.MarkAsBoarded(modifiedBy);
            return Result.Success();
        }

        public Result CancelReservation(Reservation reservation, string modifiedBy)
        {
            reservation.Cancel(modifiedBy);
            return Result.Success();
        }

        public Task<bool> PastTrip(Reservation reservation, Trip trip)
        {
            return Task.FromResult(trip.ScheduledDepartureTime < DateTime.UtcNow && reservation.Status != ReservationStatus.Confirmed);
        }
    }
}

using SGA.Domain.Entities.Trips;
using SGA.Domain.Enums.Trips;

namespace SGA.Application.Rules
{
    internal static class TripBookingRules
    {
        private static readonly TimeSpan AutoCancelThreshold = TimeSpan.FromHours(3);

        public static bool HasDeparturePassed(Trip trip, DateTime utcNow)
            => trip.ScheduledDepartureTime.ToUniversalTime() <= utcNow;

        public static bool MustAutoCancel(Trip trip, DateTime utcNow)
            => trip.Status == TripStatus.Scheduled
                && trip.ActualDepartureTime is null
                && trip.ScheduledDepartureTime.ToUniversalTime().Add(AutoCancelThreshold) <= utcNow;
    }
}
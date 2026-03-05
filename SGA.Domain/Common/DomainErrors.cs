namespace SGA.Domain.Common
{
    public static class DomainErrors
    {
        public static class General
        {
            public static readonly Error Unexpected = new("General.Unexpected", "Unexpected domain error.");
            public static readonly Error Validation = new("General.Validation", "Validation error.");
        }

        public static class Trip
        {
            public static readonly Error InvalidState = new("Trip.InvalidState", "Trip state transition is invalid.");
            public static readonly Error NotScheduled = new("Trip.NotScheduled", "Trip must be scheduled to start.");
            public static readonly Error NotInProgress = new("Trip.NotInProgress", "Trip must be in progress to complete.");
            public static readonly Error AlreadyCompleted = new("Trip.AlreadyCompleted", "Cannot cancel a completed trip.");
        }

        public static class Authorization
        {
            public static readonly Error InvalidType = new("Authorization.InvalidType", "Authorization type does not allow this operation.");
            public static readonly Error InsufficientBalance = new("Authorization.InsufficientBalance", "Authorization has insufficient balance.");

        }

        public static class Reservation
        {
            public static readonly Error InvalidState = new("Reservation.InvalidState", "Reservation state transition is invalid.");
        }

        public static class Person
        {
            public static readonly Error Unavailable = new("Driver.Unavailable", "Driver is already unavailable");
            public static readonly Error Available = new("Driver.Unavailable", "Driver is already Available");
            public static readonly Error InvalidState = new("Driver.InvalidState", "User state transition is invalid.");
            public static readonly Error LicenseExpired = new("Driver.LicenseExpired", "Driver license is expired.");
            public static readonly Error PeriodIsNull = new("Student.PeriodNull", "Driver license is expired.");
        }
    }
}

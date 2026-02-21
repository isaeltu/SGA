using System;

namespace SGA.Domain.Exceptions.Trips
{
    public class TripNotAvailableException : Exception
    {
        public TripNotAvailableException()
        {
        }

        public TripNotAvailableException(string message) : base(message)
        {
        }

        public TripNotAvailableException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

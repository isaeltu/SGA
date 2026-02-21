using System;

namespace SGA.Domain.Exceptions.Transportation
{
    public class BusNotAvailableException : Exception
    {
        public BusNotAvailableException()
        {
        }

        public BusNotAvailableException(string message) : base(message)
        {
        }

        public BusNotAvailableException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

using System;

namespace SGA.Domain.Exceptions.Users
{
    public class DriverNotAvailableException : Exception
    {
        public DriverNotAvailableException()
        {
        }

        public DriverNotAvailableException(string message) : base(message)
        {
        }

        public DriverNotAvailableException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

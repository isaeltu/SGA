using System;

namespace SGA.Domain.Exceptions.Incidents
{
    public class InvalidIncidentException : Exception
    {
        public InvalidIncidentException()
        {
        }

        public InvalidIncidentException(string message) : base(message)
        {
        }

        public InvalidIncidentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

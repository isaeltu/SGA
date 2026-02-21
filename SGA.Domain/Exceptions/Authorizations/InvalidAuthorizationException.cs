using System;

namespace SGA.Domain.Exceptions.Authorizations
{
    public class InvalidAuthorizationException : Exception
    {
        public InvalidAuthorizationException()
        {
        }

        public InvalidAuthorizationException(string message) : base(message)
        {
        }

        public InvalidAuthorizationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

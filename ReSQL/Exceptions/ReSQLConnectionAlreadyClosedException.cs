using System;
using System.Runtime.Serialization;

namespace Exceptions
{
    [Serializable]
    internal class ReSQLConnectionAlreadyClosedException : Exception
    {
        public ReSQLConnectionAlreadyClosedException()
        {
        }

        public ReSQLConnectionAlreadyClosedException(string message) : base(message)
        {
        }

        public ReSQLConnectionAlreadyClosedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ReSQLConnectionAlreadyClosedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
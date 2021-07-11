using System;
using System.Runtime.Serialization;

namespace Exceptions
{
    [Serializable]
    internal class ReSQLConnectionAlreadyOpenException : Exception
    {
        public ReSQLConnectionAlreadyOpenException()
        {
        }

        public ReSQLConnectionAlreadyOpenException(string message) : base(message)
        {
        }

        public ReSQLConnectionAlreadyOpenException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ReSQLConnectionAlreadyOpenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
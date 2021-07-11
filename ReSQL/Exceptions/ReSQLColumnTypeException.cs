using System;
using System.Runtime.Serialization;

namespace Exceptions
{
    [Serializable]
    internal class ReSQLColumnTypeException : Exception
    {
        public ReSQLColumnTypeException()
        {
        }

        public ReSQLColumnTypeException(string message) : base(message)
        {
        }

        public ReSQLColumnTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ReSQLColumnTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
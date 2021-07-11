using System;
using System.Runtime.Serialization;

namespace Exceptions
{
    [Serializable]
    internal class ReSQLEmptyTableException : Exception
    {
        public ReSQLEmptyTableException()
        {
        }

        public ReSQLEmptyTableException(string message) : base(message)
        {
        }

        public ReSQLEmptyTableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ReSQLEmptyTableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
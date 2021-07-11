using System;
using System.Runtime.Serialization;

namespace Exceptions
{
    [Serializable]
    internal class ReSQLIdentifierNotFoundException : Exception
    {
        public ReSQLIdentifierNotFoundException()
        {
        }

        public ReSQLIdentifierNotFoundException(string message) : base(message)
        {
        }

        public ReSQLIdentifierNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ReSQLIdentifierNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
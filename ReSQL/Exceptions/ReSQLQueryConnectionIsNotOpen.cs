using System;
using System.Runtime.Serialization;

namespace Exceptions
{
    [Serializable]
    internal class ReSQLQueryConnectionIsNotOpen : Exception
    {
        public ReSQLQueryConnectionIsNotOpen()
        {
        }

        public ReSQLQueryConnectionIsNotOpen(string message) : base(message)
        {
        }

        public ReSQLQueryConnectionIsNotOpen(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ReSQLQueryConnectionIsNotOpen(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
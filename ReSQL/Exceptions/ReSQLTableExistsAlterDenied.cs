using System;
using System.Runtime.Serialization;

namespace Exceptions
{
    [Serializable]
    internal class ReSQLTableExistsAlterDenied : Exception
    {
        public ReSQLTableExistsAlterDenied()
        {
        }

        public ReSQLTableExistsAlterDenied(string message) : base(message)
        {
        }

        public ReSQLTableExistsAlterDenied(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ReSQLTableExistsAlterDenied(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
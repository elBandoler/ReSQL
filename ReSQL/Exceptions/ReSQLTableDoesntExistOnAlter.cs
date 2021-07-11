using System;
using System.Runtime.Serialization;

namespace Exceptions
{
    [Serializable]
    internal class ReSQLTableDoesntExistOnAlter : Exception
    {
        public ReSQLTableDoesntExistOnAlter()
        {
        }

        public ReSQLTableDoesntExistOnAlter(string message) : base(message)
        {
        }

        public ReSQLTableDoesntExistOnAlter(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ReSQLTableDoesntExistOnAlter(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
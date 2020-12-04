using System;
using System.Runtime.Serialization;

namespace DevelApp.JsonSchemaBuilder.Exceptions
{
    [Serializable]
    internal class IdentifierStringException : Exception
    {
        public IdentifierStringException()
        {
        }

        public IdentifierStringException(string message) : base(message)
        {
        }

        public IdentifierStringException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IdentifierStringException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
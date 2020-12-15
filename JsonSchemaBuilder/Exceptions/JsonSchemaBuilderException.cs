using System;
using System.Runtime.Serialization;

namespace DevelApp.JsonSchemaBuilder.Exceptions
{
    [Serializable]
    internal class JsonSchemaBuilderException : Exception
    {
        public JsonSchemaBuilderException()
        {
        }

        public JsonSchemaBuilderException(string message) : base(message)
        {
        }

        public JsonSchemaBuilderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected JsonSchemaBuilderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
using System;
using System.Runtime.Serialization;

namespace DevelApp.JsonSchemaBuilder.Exceptions
{
    [Serializable]
    internal class CodeGenerationException : Exception
    {
        public CodeGenerationException()
        {
        }

        public CodeGenerationException(string message) : base(message)
        {
        }

        public CodeGenerationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CodeGenerationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
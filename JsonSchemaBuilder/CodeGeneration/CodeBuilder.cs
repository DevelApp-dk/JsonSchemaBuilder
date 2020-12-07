using DevelApp.JsonSchemaBuilder.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.CodeGeneration
{
    /// <summary>
    /// Wrapped StringBuilder to avoid it taking too much attention
    /// </summary>
    public sealed class CodeBuilder
    {
        private StringBuilder _stringBuilder;
        
        public CodeBuilder(int indentSpacesPerIndent = 4)
        {
            _stringBuilder = new StringBuilder();
        }

        /// <summary>
        /// Appends a line to internal string builder and returns itself to allow for chained calls
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public CodeBuilder L(string line)
        {
            _stringBuilder.AppendLine(line);
            return this;
        }

        /// <summary>
        /// Called in the end of the code build to translate builder code into string
        /// </summary>
        /// <returns></returns>
        public string Build()
        {
            if (_indent == 0)
            {
                return _stringBuilder.ToString();
            }
            else
            {
                throw new CodeGenerationException($"Indent has not been reset to zero indication an error in generated code");
            }
        }

        private int _indent = 0;

        /// <summary>
        /// Increase indent for the code
        /// </summary>
        internal void IndentIncrease()
        {
            _indent += 1;
        }

        /// <summary>
        /// Decrease indent for the code
        /// </summary>
        internal void IndentDecrease()
        {
            _indent -= 1;
        }
    }
}

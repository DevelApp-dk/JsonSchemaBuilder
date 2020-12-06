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
        
        public CodeBuilder()
        {
            _stringBuilder = new StringBuilder();
        }

        /// <summary>
        /// Appends a line to internal string builder and returns itself to allow for chaned calls
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public CodeBuilder L(string line)
        {
            _stringBuilder.AppendLine(line);
            return this;
        }

        public string Build()
        {
            return _stringBuilder.ToString();
        }
    }
}

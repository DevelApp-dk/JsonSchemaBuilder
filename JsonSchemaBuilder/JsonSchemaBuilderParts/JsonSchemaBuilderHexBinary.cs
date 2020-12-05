using DevelApp.JsonSchemaBuilder.Model;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    /// <summary>
    /// Convenience hexbinary definition in Json Schema.
    /// </summary>
    public class JsonSchemaBuilderHexBinary : JsonSchemaBuilderString
    {
        public JsonSchemaBuilderHexBinary(
                IdentifierString timeName,
                string description,
                uint? charSize = null,
                bool isRequired = false)
            : base(timeName,
                description,
                format: "hexbinary",
                minLength: charSize.HasValue ? charSize.Value : 0,
                maxLength: charSize,
                pattern: "^([0-9a-fA-F]{2})*$",
                isRequired: isRequired)
        {
        }

        public override JsonSchemaBuilderPartType PartType
        {
            get
            {
                return JsonSchemaBuilderPartType.HexBinary;
            }
        }
    }
}

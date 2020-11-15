using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder
{
    public interface IJsonSchemaBuilderPart
    {
        /// <summary>
        /// Returns JsonSchemaPart as Manatee JsonSchema
        /// </summary>
        /// <returns></returns>
        JsonSchema AsJsonSchema();

        /// <summary>
        /// Returns the PartType of the JsonSchemaPart
        /// </summary>
        JsonSchemaBuilderPartType PartType { get; }
    }
}

using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using DevelApp.JsonSchemaBuilder.Model;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder
{
    public interface IJsonSchemaBuilderPart
    {
        /// <summary>
        /// Name of the schema part
        /// </summary>
        IdentifierString Name { get; }

        /// <summary>
        /// Desciption of the schema part
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Is the schema part required
        /// </summary>
        bool IsRequired { get; }

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

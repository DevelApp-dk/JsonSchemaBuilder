using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder
{
    public interface IJsonSchemaDefinition
    {
        /// <summary>
        /// Returns the name of the schema
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns the module of the schema
        /// </summary>
        string Module { get; }

        /// <summary>
        /// The description of the schema
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Returns the complete JsonSchema
        /// </summary>
        JsonSchema JsonSchema { get; }

        /// <summary>
        /// Returns JsonSchemaBuilderSchema
        /// </summary>
        JsonSchemaBuilderSchema JsonSchemaBuilderSchema { get; }

        /// <summary>
        /// Writes the schema to filePath
        /// </summary>
        /// <param name="filePath"></param>
        void WriteSchemaToFile(string filePath);
    }
}

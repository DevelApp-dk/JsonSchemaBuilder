using DevelApp.JsonSchemaBuilder.CodeGeneration;
using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using DevelApp.JsonSchemaBuilder.Model;
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
        IdentifierString Name { get; }

        /// <summary>
        /// Returns the module of the schema. This works as a namespace with dot separating namespace and subnamespace
        /// </summary>
        NamespaceString Module { get; }

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
        /// <param name="filePathBeforeModuleNamespace"></param>
        void WriteSchemaToFile(string filePathBeforeModuleNamespace);

        /// <summary>
        /// Generate code to memory and suggest a filename
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        (string fileName, string code) GenerateCode(Code code);

        /// <summary>
        /// Generate code to a path
        /// </summary>
        /// <param name="code"></param>
        /// <param name="filePathBeforeModuleNamespace"></param>
        void GenerateCode(Code code, string filePathBeforeModuleNamespace);
    }
}

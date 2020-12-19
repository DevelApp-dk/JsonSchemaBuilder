using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using DevelApp.Utility.Model;
using Manatee.Json.Schema;

namespace DevelApp.JsonSchemaBuilder
{
    public interface IJSBPart
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
        JSBPartType PartType { get; }
    }
}

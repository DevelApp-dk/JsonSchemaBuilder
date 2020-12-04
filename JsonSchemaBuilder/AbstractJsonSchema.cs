using System;
using System.Collections.Generic;
using Manatee.Json;
using Manatee.Json.Schema;
using Manatee.Json.Serialization;
using System.IO;
using DevelApp.JsonSchemaBuilder.JsonSchemaParts;

namespace DevelApp.JsonSchemaBuilder
{
    /// <summary>
    /// Schema builder abstract class with convenience methosd for creating JsonSchema
    /// </summary>
    public abstract class AbstractJsonSchema: IJsonSchemaDefinition
    {
        #region Implemented Parts

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonSchema"></param>
        /// <param name="jsonValue"></param>
        public AbstractJsonSchema(JsonSchema jsonSchema = null, JsonValue jsonValue = null)
        {
            if (jsonSchema != null)
            {
                JsonSchemaBuilderSchema = BuildJsonSchema(jsonSchema);
            }
            else if (jsonValue != null)
            {
                JsonSchemaBuilderSchema = BuildJsonSchema(jsonValue);
            }
            else
            {
                JsonSchemaBuilderSchema = BuildJsonSchema();
            }
        }

        private JsonSchema _jsonSchema;

        /// <summary>
        /// Returns the complete Manatee JsonSchema
        /// </summary>
        public JsonSchema JsonSchema { 
            get
            {
                if (_jsonSchema == null)
                {
                    _jsonSchema =  JsonSchemaBuilderSchema.AsJsonSchema();
                }
                return _jsonSchema;
            }
        }

        /// <summary>
        /// Returns JsonSchemaBuilderSchema for extending
        /// </summary>
        public JsonSchemaBuilderSchema JsonSchemaBuilderSchema { get; }

        /// <summary>
        /// Name of the class without JsonSchema if existing
        /// </summary>
        public string Name
        {
            get
            {
                string name = GetType().Name;
                string nameWithoutJsonSchema = name.Replace("JsonSchema", "");

                return $"{Module}.{nameWithoutJsonSchema}";
            }
        }

        /// <summary>
        /// Write schema to file
        /// </summary>
        /// <param name="filePath"></param>
        public void WriteSchemaToFile(string filePath)
        {
            var serializer = new JsonSerializer();
            if (JsonSchema != null)
            {
                var schemaInJson = JsonSchema.ToJson(serializer);
                string fileName = $"{TransformToCorrectCase(Module)}.{TransformToCorrectCase(Name)}{FileEnding}";

                File.WriteAllText(Path.Combine(filePath, fileName), schemaInJson.GetIndentedString());
            }
            else
            {
                throw new Exception("WriteSchemaToFile called before JsonSchema has been set");
            }
        }

        #endregion

        #region Abstract Parts

        /// <summary>
        /// Returns the module of the schema
        /// </summary>
        public abstract string Module { get; }

        /// <summary>
        /// The description of the schema
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Main function. Used to build JsonSchemaBuilder from JsonSchema
        /// </summary>
        /// <returns></returns>
        protected abstract JsonSchemaBuilderSchema BuildJsonSchema(JsonSchema jsonSchema);

        /// <summary>
        /// Main function. Used to build JsonSchema from JsonSchemaParts
        /// </summary>
        /// <returns></returns>
        protected virtual JsonSchemaBuilderSchema BuildJsonSchema(JsonValue jsonValue)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();
            JsonSchema deserializedSchema = jsonSerializer.Deserialize<JsonSchema>(jsonValue);
            return BuildJsonSchema(deserializedSchema);
        }

        /// <summary>
        /// Main function. Used to build JsonSchema from JsonSchemaParts 
        /// if JsonSchema or JsonValue is not supplied to constructor
        /// </summary>
        /// <returns></returns>
        protected abstract JsonSchemaBuilderSchema BuildJsonSchema();

        #endregion


        private string TransformToCorrectCase(string stringToTransform)
        {
            return stringToTransform.Substring(0, 1).ToLowerInvariant() + stringToTransform.Substring(1);
        }

        /// <summary>
        /// Almost standard file ending
        /// </summary>
        protected string FileEnding
        {
            get
            {
                return ".schema.json";
            }
        }


        #region JsonSchemaBuilder
        // Inspiration for types from https://github.com/lcahlander/xsd2json





        #endregion
    }
}

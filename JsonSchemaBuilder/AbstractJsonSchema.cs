using System;
using System.Collections.Generic;
using Manatee.Json;
using Manatee.Json.Schema;
using Manatee.Json.Serialization;
using System.IO;

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
                JsonSchemaPart = BuildJsonSchema(jsonSchema);
            }
            else if (jsonValue != null)
            {
                JsonSchemaPart = BuildJsonSchema(jsonValue);
            }
            else
            {
                JsonSchemaPart = BuildJsonSchema();
            }
        }

        /// <summary>
        /// Returns the complete Manatee JsonSchema
        /// </summary>
        public JsonSchema JsonSchema { 
            get
            {
                return JsonSchemaPart.AsJsonSchema();
            }
        }

        /// <summary>
        /// Returns JsonSchemaPart
        /// </summary>
        public IJsonSchemaBuilderPart JsonSchemaPart { get; }

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
        /// Main function. Used to build JsonSchema from JsonSchema
        /// </summary>
        /// <returns></returns>
        protected virtual IJsonSchemaBuilderPart BuildJsonSchema(JsonSchema jsonSchema)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();
            JsonValue serializedSchema = jsonSerializer.Serialize(jsonSchema);
            return BuildJsonSchema(serializedSchema);
        }

        /// <summary>
        /// Main function. Used to build JsonSchema from JsonSchemaParts
        /// </summary>
        /// <returns></returns>
        protected virtual IJsonSchemaBuilderPart BuildJsonSchema(JsonValue jsonValue)
        {
            //TODO Do something
        }

        /// <summary>
        /// Main function. Used to build JsonSchema from JsonSchemaParts 
        /// if JsonSchema or JsonValue is not supplied to constructor
        /// </summary>
        /// <returns></returns>
        protected abstract IJsonSchemaBuilderPart BuildJsonSchema();

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


        /// <summary>
        /// Convenience schema definition. TopObject means this is the root of the schema. Expandable is used to define if schema can be inherited
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="topHierarchy">Is the top of hierarchy</param>
        /// <param name="expandable"></param>
        /// <returns></returns>
        protected JsonSchema Object(string title, string description, bool topHierarchy = false, bool expandable = false)
        {
            return Factory(topHierarchy)
                .Type(JsonSchemaType.Object)
                .Title(title)
                .Description(description)
                .AdditionalProperties(expandable);
        }

        /// <summary>
        /// Convenience string definition in Json Schema.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="topHierarchy">Is the top of hierarchy</param>
        /// <returns></returns>
        protected JsonSchema String(string title, string description, bool topHierarchy = false)
        {
            return Factory(topHierarchy)
                .Type(JsonSchemaType.String)
                .Title(title)
                .Description(description);
        }

        /// <summary>
        /// Convenience integer definition in Json Schema.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="topHierarchy">Is the top of hierarchy</param>
        /// <returns></returns>
        protected JsonSchema Integer(string title, string description, bool topHierarchy = false)
        {
            return Factory(topHierarchy)
                .Type(JsonSchemaType.Integer)
                .Title(title)
                .Description(description);
        }


        /// <summary>
        /// Convenience number definition in Json Schema.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        protected JsonSchema Number(string title, string description, bool topHierarchy = false)
        {
            return Factory(topHierarchy)
                .Type(JsonSchemaType.Number)
                .Title(title)
                .Description(description);
        }

        /// <summary>
        /// Convenience enum definition in Json Schema. First value en enumString is the default
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="enumStrings"></param>
        /// <returns></returns>
        protected JsonSchema Enum(string title, string description, bool topHierarchy = false, params string[] enumStrings)
        {
            List<JsonValue> enumJsonValues = new List<JsonValue>();
            foreach (string enumString in enumStrings)
            {
                enumJsonValues.Add(enumString);
            }

            return Factory(topHierarchy)
                .Type(JsonSchemaType.String)
                .Title(title)
                .Description(description)
                .Enum(enumJsonValues.ToArray());
        }

        /// <summary>
        /// Convenience hexbinary definition in Json Schema.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="charSize"></param>
        /// <returns></returns>
        protected JsonSchema HexBinary(string title, string description, int charSize, bool topHierarchy = false)
        {
            return Factory(topHierarchy)
                .Type(JsonSchemaType.String)
                .Title(title)
                .Description(description)
                .MinLength((uint)charSize)
                .MaxLength((uint)charSize)
                .Pattern("^([0-9a-fA-F]{2})*$");
        }

        /// <summary>
        /// Comvinience email definition in Json Schema.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        protected JsonSchema Email(string title, string description, bool topHierarchy = false)
        {
            return Factory(topHierarchy)
                .Type(JsonSchemaType.String)
                .Title(title)
                .Description(description)
                .Pattern("[a-z0-9\\._%+!$&*=^|~#%{}/\\-]+@([a-z0-9\\-]+\\.){1,}([a-z]{2,22})");
        }

        /// <summary>
        /// Add reference "$ref": "./xs.schema.json#/definitions/xs:decimal" with "./xs.schema.json" as the local file and "#/definitions/xs:decimal" getting xs:decimal from the definition
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="objectReference"></param>
        /// <param name="fileLocation"></param>
        /// <returns></returns>
        protected JsonSchema Reference(string title, string description, string objectReference, string fileLocation = "", bool topHierarchy = false)
        {
            if (string.IsNullOrWhiteSpace(fileLocation))
            {
                return Factory(topHierarchy)
                        .Type(JsonSchemaType.Object)
                    .Title(title)
                    .Description(description)
                    .Ref(objectReference);
            }
            else
            {
                return Factory(topHierarchy)
                        .Type(JsonSchemaType.Object)
                    .Title(title)
                    .Description(description)
                    .Ref(fileLocation + objectReference);
            }
        }

        #endregion
    }
}

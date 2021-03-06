﻿using System;
using Manatee.Json;
using Manatee.Json.Schema;
using Manatee.Json.Serialization;
using System.IO;
using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using DevelApp.JsonSchemaBuilder.CodeGeneration;
using DevelApp.Utility.Model;
using System.Linq;

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

        protected JsonSchema _jsonSchema;

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
        public JSBSchema JsonSchemaBuilderSchema { get; }

        /// <summary>
        /// Name of the class without JsonSchema if existing
        /// </summary>
        public IdentifierString Name
        {
            get
            {
                return GetType().Name.Replace("JsonSchema", "");
            }
        }

        /// <summary>
        /// Write schema to file
        /// </summary>
        /// <param name="applicationRoot"></param>
        public void WriteSchemaToFile(string applicationRoot)
        {
            var serializer = new JsonSerializer();
            if (JsonSchema != null)
            {
                var schemaInJson = JsonSchema.ToJson(serializer);
                string localFile = Path.Combine(applicationRoot, FileName);
                FileInfo fileInfo = new FileInfo(localFile);
                string directory = fileInfo.Directory.FullName;
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(localFile, schemaInJson.GetIndentedString());
            }
            else
            {
                throw new Exception("WriteSchemaToFile called before JsonSchema has been set");
            }
        }

        private CodeGenerator CodeGeneration(string applicationRoot, string jsonSchemaApplicationRoot)
        {
            CodeGenerator codeGenerator = new CodeGenerator(applicationRoot, jsonSchemaApplicationRoot);
            codeGenerator.Register(this);
            return codeGenerator;
        }

        /// <summary>
        /// Generate code to memory and suggest a filename
        /// </summary>
        /// <param name="code"></param>
        /// <param name="applicationRoot"></param>
        /// <returns></returns>
        public (string fileName, string code) GenerateCode(Code code, string applicationRoot, string jsonSchemaApplicationRoot)
        {
            return CodeGeneration(applicationRoot, jsonSchemaApplicationRoot).Generate(code).FirstOrDefault();
        }

        /// <summary>
        /// Generate code to a path
        /// </summary>
        /// <param name="code"></param>
        /// <param name="applicationRoot"></param>
        public void GenerateCodeToFile(Code code, string applicationRoot, string jsonSchemaApplicationRoot)
        {
            CodeGeneration(applicationRoot, jsonSchemaApplicationRoot).GenerateToFile(code);
        }

        public string FileName
        {
            get
            {
                return Path.Combine(Module.ToFilePath, TransformToCamelCase(Name) + FILE_ENDING);
            }
        }

        #endregion

        #region Abstract Parts

        /// <summary>
        /// Returns the module of the schema
        /// </summary>
        public abstract NamespaceString Module { get; }

        /// <summary>
        /// The description of the schema
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Main function. Used to build JsonSchemaBuilder from JsonSchema
        /// </summary>
        /// <returns></returns>
        protected JSBSchema BuildJsonSchema(JsonSchema jsonSchema)
        {
            return JSBSchema.BuildSchema(jsonSchema);
        }

        /// <summary>
        /// Main function. Used to build JsonSchema from JsonSchemaParts
        /// </summary>
        /// <returns></returns>
        protected virtual JSBSchema BuildJsonSchema(JsonValue jsonValue)
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
        protected abstract JSBSchema BuildJsonSchema();

        #endregion


        private string TransformToCamelCase(string stringToTransform)
        {
            return stringToTransform.Substring(0, 1).ToLowerInvariant() + stringToTransform.Substring(1);
        }

        /// <summary>
        /// Almost standard file ending
        /// </summary>
        protected const string FILE_ENDING =  ".schema.json";
    }
}

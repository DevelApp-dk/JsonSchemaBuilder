using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using DevelApp.Utility.Model;
using System;
using System.Collections.Generic;

namespace DevelApp.JsonSchemaBuilder.CodeGeneration
{
    public class CSharp
    {
        private const string FILE_ENDING = ".cs";

        /// <summary>
        /// Generate fileName and code
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        internal static (string fileName, string code) GenerateCode(string fileLocation, IJsonSchemaDefinition schema)
        {
            string fileName = fileLocation + FILE_ENDING;

            CSharp cSharp = new CSharp(schema.Module);
            return (fileName, cSharp.GenerateCode(schema.JsonSchemaBuilderSchema));
        }

        private NamespaceString _startNameSpace;
        private CSharp(NamespaceString startNameSpace)
        {
            _startNameSpace = startNameSpace;
        }

        /// <summary>
        /// Generate code from JsonSchemaBuilderSchema
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        private string GenerateCode(JsonSchemaBuilderSchema schema)
        {
            CodeBuilder codeBuilder = new CodeBuilder();
            GenerateStartOfSchema(codeBuilder, schema);
            if (schema.TopPart.PartType == JsonSchemaBuilderPartType.Object)
            {
                GenerateCodeForBuilderPart(codeBuilder, schema.TopPart.Name, schema.TopPart, schema.Definitions);
            }
            else
            {
                Dictionary<IdentifierString, IJsonSchemaBuilderPart> properties = new Dictionary<IdentifierString, IJsonSchemaBuilderPart>();
                properties.Add(schema.Name, schema.TopPart);

                JsonSchemaBuilderObject encasingObject = new JsonSchemaBuilderObject(schema.TopPart.Name, schema.TopPart.Description, properties: properties);
                GenerateCodeForBuilderPart(codeBuilder, schema.TopPart.Name, encasingObject, schema.Definitions);
            }
            GenerateEndOfSchema(codeBuilder, schema);
            return codeBuilder.Build();
        }

        /// <summary>
        /// Generates using parameters and initial class specification
        /// </summary>
        /// <param name="codeBuilder"></param>
        /// <param name="schema"></param>
        private void GenerateStartOfSchema(CodeBuilder codeBuilder, JsonSchemaBuilderSchema schema)
        {
            //TODO run through schema children to get all references to make sure they are included
            codeBuilder
                .L("using System;")
                .L("using Newtonsoft.Json;")
                .EmptyLine()
                .L($"namespace {_startNameSpace}")
                .L("{")
                .IndentIncrease();
        }

        private void GenerateEndOfSchema(CodeBuilder codeBuilder, JsonSchemaBuilderSchema schema)
        {
            codeBuilder
                .IndentDecrease()
                .L("}");
        }

        private void GenerateCodeForBuilderPart(CodeBuilder codeBuilder, IdentifierString key, IJsonSchemaBuilderPart value, Dictionary<IdentifierString, IJsonSchemaBuilderPart> definitions = null)
        {
            switch (value.PartType)
            {
                case JsonSchemaBuilderPartType.Object:

                    JsonSchemaBuilderObject jsonSchemaBuilderObject = value as JsonSchemaBuilderObject;
                    codeBuilder
                        //TODO Add comment from description split on lines
                        .L($"public partial class {TransformToTitleCase(key)}")
                        .L("{")
                        .IndentIncrease();

                    //Add definitions
                    if (definitions != null)
                    {
                        foreach (KeyValuePair<IdentifierString, IJsonSchemaBuilderPart> pair in definitions)
                        {
                            GenerateCodeForBuilderPart(codeBuilder, pair.Key, pair.Value);
                            codeBuilder.EmptyLine();
                        }
                    }

                    //TODO Add own code

                    //Add children
                    foreach(KeyValuePair<IdentifierString, IJsonSchemaBuilderPart> pair in jsonSchemaBuilderObject.Properties)
                    {
                        GenerateCodeForBuilderPart(codeBuilder, pair.Key, pair.Value);
                        codeBuilder.EmptyLine();
                    }

                    codeBuilder
                        .IndentDecrease()
                        .L("}");
                    break;

                case JsonSchemaBuilderPartType.String:
                    JsonSchemaBuilderString jsonSchemaBuilderString = value as JsonSchemaBuilderString;
                    codeBuilder
                        .L($"[JsonProperty(\"{TransformToCamelCase(key)}\")]")
                        .L($"public string {TransformToTitleCase(key)} {{ get; set; }}")
                        .EmptyLine();
                    break;
                default:
                    codeBuilder.L($"throw new NotImplementedException(\"PartType {value.PartType} is not implemented\");");
                    break;
            }
        }

        /// <summary>
        /// Transform string from TitleCase to camelCase
        /// </summary>
        /// <param name="stringToTransform"></param>
        /// <returns></returns>
        private string TransformToCamelCase(string stringToTransform)
        {
            return stringToTransform.Substring(0, 1).ToLowerInvariant() + stringToTransform.Substring(1);
        }

        /// <summary>
        /// Transform string from camelCase to TitleCase
        /// </summary>
        /// <param name="stringToTransform"></param>
        /// <returns></returns>
        private string TransformToTitleCase(string stringToTransform)
        {
            return stringToTransform.Substring(0, 1).ToUpperInvariant() + stringToTransform.Substring(1);
        }

    }
}

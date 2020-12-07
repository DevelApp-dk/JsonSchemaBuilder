using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using DevelApp.JsonSchemaBuilder.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
            if (schema.Definitions.Count > 0)
            {
                codeBuilder.IndentIncrease();
                GenerateStartOfDefinitions(codeBuilder);
                codeBuilder.IndentIncrease();
                foreach (KeyValuePair<IdentifierString, IJsonSchemaBuilderPart> pair in schema.Definitions)
                {
                    GenerateCodeForBuilderPart(codeBuilder, pair.Key, pair.Value);
                }
                codeBuilder.IndentDecrease();
                GenerateEndOfDefinitions(codeBuilder);
                codeBuilder.IndentDecrease();
            }
            GenerateEndOfSchema(codeBuilder, schema);
            return codeBuilder.Build();
        }
    }
}

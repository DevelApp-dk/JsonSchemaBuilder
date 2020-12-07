using DevelApp.JsonSchemaBuilder.Exceptions;
using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.CodeGeneration
{
    public class GenerateCode
    {
        /// <summary>
        /// Generate code to memory and suggest a filename
        /// </summary>
        /// <param name="code"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        public (string fileName, string code) Generate(Code code, IJsonSchemaDefinition schema)
        {
            string fileName = Path.Combine(schema.Module.ToFilePath, schema.Name);

            switch (code)
            {
                case Code.CSharp:
                    return CSharp.GenerateCode(fileName, schema);
                default:
                    throw new CodeGenerationException($"Code generation of {code} is not supported");
            }
        }

        /// <summary>
        /// Generate code to a path
        /// </summary>
        /// <param name="code"></param>
        /// <param name="schema"></param>
        /// <param name="filePathBeforeNamespace"></param>
        public void Generate(Code code, IJsonSchemaDefinition schema, string filePathBeforeNamespace)
        {
            var tuple = Generate(code, schema);
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePathBeforeNamespace, tuple.fileName)))
            {
                outputFile.Write(tuple.code);
                outputFile.Flush();
            }
        }
    }
}

using DevelApp.JsonSchemaBuilder.Exceptions;
using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.CodeGeneration
{
    public class CodeGenerator
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
            string fullPath = Path.Combine(filePathBeforeNamespace, tuple.fileName);

            FileInfo fileInfo = new FileInfo(fullPath);
            string directory = fileInfo.Directory.FullName;
            try
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (Exception ex)
            {
                throw new CodeGenerationException($"Could not create path {directory}", ex);
            }

            try
            {
                using (StreamWriter outputFile = new StreamWriter(fullPath))
                {
                    outputFile.Write(tuple.code);
                    outputFile.Flush();
                }
            }
            catch(Exception ex)
            {
                throw new CodeGenerationException($"Could not write code file to {fullPath}", ex);
            }
        }
    }
}

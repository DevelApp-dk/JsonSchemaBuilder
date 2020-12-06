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
        public (string fileName, string code) Generate(Code code, JsonSchemaBuilderSchema schema)
        {
            switch (code)
            {
                case Code.CSharp:
                    return CSharp.GenerateCode(schema);
                default:
                    throw new CodeGenerationException($"Code generation of {code} is not supported");
            }
        }

        public void Generate(Code code, JsonSchemaBuilderSchema schema, string folderLocation)
        {
            var tuple = Generate(code, schema);
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(folderLocation, tuple.fileName)))
            {
                outputFile.Write(tuple.code);
                outputFile.Flush();
            }
        }
    }
}

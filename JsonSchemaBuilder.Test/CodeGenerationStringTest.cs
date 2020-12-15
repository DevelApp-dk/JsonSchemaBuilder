using DevelApp.JsonSchemaBuilder;
using DevelApp.JsonSchemaBuilder.CodeGeneration;
using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using DevelApp.Utility.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace JsonSchemaBuilder.Test
{
    public class CodeGenerationStringTest
    {
        [Fact]
        public void StringAsTopPart()
        {
            string pathString = "E:\\\\Projects\\JsonSchemaBuilder\\ModelTest\\";

            IJsonSchemaDefinition jsonSchemaDefinition = new StringAsTopPartJsonSchema();
            jsonSchemaDefinition.WriteSchemaToFile(pathString);
            CodeGenerator codeGenerator = new CodeGenerator();
            codeGenerator.Generate(Code.CSharp, jsonSchemaDefinition, pathString);
        }
    }


    public class StringAsTopPartJsonSchema : AbstractJsonSchema
    {
        public override NamespaceString Module
        {
            get
            {
                return "Funny.Onion";
            }
        }

        public override string Description
        {
            get
            {
                return "Used to test string as a top part";
            }
        }

        protected override JsonSchemaBuilderSchema BuildJsonSchema()
        {
            JsonSchemaBuilderString stringPart = new JsonSchemaBuilderString("MyTopPartString", "TopPart");

            return new JsonSchemaBuilderSchema("StringAsATopPart", Description, topPart: stringPart);
        }
    }
}

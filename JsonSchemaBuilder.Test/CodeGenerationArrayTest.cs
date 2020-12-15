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
    public class CodeGenerationArrayTest
    {
        //[Fact]
        //public void ArrayAsTopPart()
        //{
        //    string pathString = "E:\\\\Projects\\JsonSchemaBuilder\\ModelTest\\";

        //    IJsonSchemaDefinition jsonSchemaDefinition = new ArrayAsTopPartJsonSchema();
        //    jsonSchemaDefinition.WriteSchemaToFile(pathString);
        //    CodeGenerator codeGenerator = new CodeGenerator();
        //    codeGenerator.Generate(Code.CSharp, jsonSchemaDefinition, pathString);
        //}
    }


    public class ArrayAsTopPartJsonSchema : AbstractJsonSchema
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
            List<IJsonSchemaBuilderPart> items = new List<IJsonSchemaBuilderPart>();

            items.Add(new JsonSchemaBuilderInteger("SwanNumber", "Swans are relevant in the world"));

            JsonSchemaBuilderArray arrayPart = new JsonSchemaBuilderArray("MyTopPartArray", "TopPart", items);

            return new JsonSchemaBuilderSchema("ArrayAsATopPart", Description, topPart: arrayPart);
        }
    }
}

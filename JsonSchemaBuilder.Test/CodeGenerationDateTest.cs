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
    public class CodeGenerationDateTest
    {
        //[Fact]
        //public void DateAsTopPart()
        //{
        //    string applicationRoot = "E:\\\\Projects\\JsonSchemaBuilder\\ModelTest\\";

        //    IJsonSchemaDefinition jsonSchemaDefinition = new DateAsTopPartJsonSchema();
        //    jsonSchemaDefinition.WriteSchemaToFile(applicationRoot);
        //    CodeGenerator codeGenerator = new CodeGenerator(applicationRoot);
        //    codeGenerator.GenerateToFile(Code.CSharp, jsonSchemaDefinition);
        //}
    }


    public class DateAsTopPartJsonSchema : AbstractJsonSchema
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
                return "Used to test date as a top part";
            }
        }

        protected override JSBSchema BuildJsonSchema()
        {
            JSBDate datePart = new JSBDate("MyTopPartDate", "TopPart", defaultValue: new DateTime(2015, 05, 16));

            return new JSBSchema("DateAsATopPart", Description, topPart: datePart);
        }
    }
}

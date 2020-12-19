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
    public class CodeGenerationTimeTest
    {
        //[Fact]
        //public void TimeAsTopPart()
        //{
        //    string pathString = "E:\\\\Projects\\JsonSchemaBuilder\\ModelTest\\";

        //    IJsonSchemaDefinition jsonSchemaDefinition = new TimeAsTopPartJsonSchema();
        //    jsonSchemaDefinition.WriteSchemaToFile(pathString);
        //    CodeGenerator codeGenerator = new CodeGenerator();
        //    codeGenerator.Generate(Code.CSharp, jsonSchemaDefinition, pathString);
        //}
    }


    public class TimeAsTopPartJsonSchema : AbstractJsonSchema
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
                return "Used to test time as a top part";
            }
        }

        protected override JSBSchema BuildJsonSchema()
        {
            JSBTime timePart = new JSBTime("MyTopPartTime", "TopPart", defaultValue: new DateTime(2020,1,1,15,30,48,765));

            return new JSBSchema("TimeAsATopPart", Description, topPart: timePart);
        }
    }
}

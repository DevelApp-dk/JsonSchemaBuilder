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
    public class CodeGenerationEmailTest
    {
        //[Fact]
        //public void EmailAsTopPart()
        //{
        //    string pathString = "E:\\\\Projects\\JsonSchemaBuilder\\ModelTest\\";

        //    IJsonSchemaDefinition jsonSchemaDefinition = new EmailAsTopPartJsonSchema();
        //    jsonSchemaDefinition.WriteSchemaToFile(pathString);
        //    CodeGenerator codeGenerator = new CodeGenerator();
        //    codeGenerator.Generate(Code.CSharp, jsonSchemaDefinition, pathString);
        //}
    }


    public class EmailAsTopPartJsonSchema : AbstractJsonSchema
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
                return "Used to test email as a top part";
            }
        }

        protected override JSBSchema BuildJsonSchema()
        {
            JSBEmail emailPart = new JSBEmail("MyTopPartEmail", "TopPart", defaultValue:"riger@support.com");

            return new JSBSchema("EmailAsATopPart", Description, topPart: emailPart);
        }
    }
}

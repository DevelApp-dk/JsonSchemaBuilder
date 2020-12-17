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
    public class CodeGenerationUriReferenceTest
    {
        //[Fact]
        //public void UriReferenceAsTopPart()
        //{
        //    string applicationRoot = "E:\\\\Projects\\JsonSchemaBuilder\\ModelTest\\";

        //    IJsonSchemaDefinition jsonSchemaDefinition = new UriReferenceAsTopPartJsonSchema();
        //    jsonSchemaDefinition.WriteSchemaToFile(applicationRoot);
        //    CodeGenerator codeGenerator = new CodeGenerator(applicationRoot);
        //    codeGenerator.GenerateToFile(Code.CSharp, jsonSchemaDefinition);
        //}
    }


    public class UriReferenceAsTopPartJsonSchema : AbstractJsonSchema
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
                return "Used to test uri reference as a top part";
            }
        }

        protected override JsonSchemaBuilderSchema BuildJsonSchema()
        {
            if (Uri.TryCreate("./dateAsTopPart", UriKind.RelativeOrAbsolute, out Uri uri))
            {

                JsonSchemaBuilderIriReference uriReferencePart = new JsonSchemaBuilderIriReference("MyTopPartUriReference", "TopPart", iriReference: uri);

                return new JsonSchemaBuilderSchema("UriReferencesATopPart", Description, topPart: uriReferencePart);
            }
            else
            {
                throw new Exception("Uri not valid");
            }
        }
    }
}

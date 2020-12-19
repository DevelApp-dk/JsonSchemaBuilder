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
    public class CodeGenerationArrayOfObjectsTest
    {
        //[Fact]
        //public void ArrayOfObjectsAsTopPart()
        //{
        //    string pathString = "E:\\\\Projects\\JsonSchemaBuilder\\ModelTest\\";

        //    IJsonSchemaDefinition jsonSchemaDefinition = new ArrayOfObjectsAsTopPartJsonSchema();
        //    jsonSchemaDefinition.WriteSchemaToFile(pathString);
        //    CodeGenerator codeGenerator = new CodeGenerator();
        //    codeGenerator.Generate(Code.CSharp, jsonSchemaDefinition, pathString);
        //}
    }

    public class ArrayOfObjectsAsTopPartJsonSchema : AbstractJsonSchema
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
                return "Used to test object as a top part";
            }
        }

        protected override JSBSchema BuildJsonSchema()
        {
            JSBBoolean booleanPart = new JSBBoolean("BooleanPart", "BooleanPart for testing", isRequired: true);
            JSBInteger integerPart = new JSBInteger("IntegerPart", "IntegerPart for testing");

            List<IJSBPart> properties = new List<IJSBPart>();
            properties.Add(booleanPart);
            properties.Add(integerPart);

            JSBObject objectPart = new JSBObject("ObjectInAnArray", "ObjectInAnArray is fun", props: properties);

            List<IJSBPart> items = new List<IJSBPart>();
            items.Add(objectPart);
            JSBArray arrayPart = new JSBArray("MyTopPartArray", "TopPart", items);

            return new JSBSchema("ArrayOfObjectsAsATopPart", Description, topPart: arrayPart);
        }
    }
}

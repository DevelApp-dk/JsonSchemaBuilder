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
    public class CodeGenerationArrayOfArrayOfObjectsTest
    {
        //[Fact]
        //public void ArrayOfArrayOfObjectsAsTopPart()
        //{
        //    string pathString = "E:\\\\Projects\\JsonSchemaBuilder\\ModelTest\\";

        //    IJsonSchemaDefinition jsonSchemaDefinition = new ArrayOfArrayOfObjectsAsTopPartJsonSchema();
        //    jsonSchemaDefinition.WriteSchemaToFile(pathString);
        //    CodeGenerator codeGenerator = new CodeGenerator();
        //    codeGenerator.Generate(Code.CSharp, jsonSchemaDefinition, pathString);
        //}
    }

    public class ArrayOfArrayOfObjectsAsTopPartJsonSchema : AbstractJsonSchema
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
                return "Used to test array of arry of object as a top part";
            }
        }

        protected override JSBSchema BuildJsonSchema()
        {
            JSBBoolean booleanPart = new JSBBoolean("BooleanPart", "BooleanPart for testing", isRequired: true);
            JSBInteger integerPart = new JSBInteger("IntegerPart", "IntegerPart for testing");

            List<IJSBPart> properties = new List<IJSBPart>();
            properties.Add(booleanPart);
            properties.Add(integerPart);

            JSBObject objectPart = new JSBObject("ObjectInAnArrayOfAnArray", "ObjectInAnArrayOfAnArray is fun", props: properties);

            List<IJSBPart> innerItems = new List<IJSBPart>();
            innerItems.Add(objectPart);
            JSBArray innerArrayPart = new JSBArray("InnerArray", "InnerArrayPart", innerItems);

            List<IJSBPart> items = new List<IJSBPart>();
            items.Add(innerArrayPart);
            JSBArray arrayPart = new JSBArray("MyTopPartArrayOfArray", "TopPart", items);

            return new JSBSchema("ArrayOfObjectsAsATopPart", Description, topPart: arrayPart);
        }
    }
}

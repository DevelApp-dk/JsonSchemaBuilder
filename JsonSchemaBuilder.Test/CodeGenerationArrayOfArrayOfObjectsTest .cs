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

        protected override JsonSchemaBuilderSchema BuildJsonSchema()
        {
            JsonSchemaBuilderBoolean booleanPart = new JsonSchemaBuilderBoolean("BooleanPart", "BooleanPart for testing", isRequired: true);
            JsonSchemaBuilderInteger integerPart = new JsonSchemaBuilderInteger("IntegerPart", "IntegerPart for testing");

            Dictionary<IdentifierString, IJsonSchemaBuilderPart> properties = new Dictionary<IdentifierString, IJsonSchemaBuilderPart>();
            properties.Add(booleanPart.Name, booleanPart);
            properties.Add(integerPart.Name, integerPart);

            JsonSchemaBuilderObject objectPart = new JsonSchemaBuilderObject("ObjectInAnArrayOfAnArray", "ObjectInAnArrayOfAnArray is fun", properties: properties);

            List<IJsonSchemaBuilderPart> innerItems = new List<IJsonSchemaBuilderPart>();
            innerItems.Add(objectPart);
            JsonSchemaBuilderArray innerArrayPart = new JsonSchemaBuilderArray("InnerArray", "InnerArrayPart", innerItems);

            List<IJsonSchemaBuilderPart> items = new List<IJsonSchemaBuilderPart>();
            items.Add(innerArrayPart);
            JsonSchemaBuilderArray arrayPart = new JsonSchemaBuilderArray("MyTopPartArrayOfArray", "TopPart", items);

            return new JsonSchemaBuilderSchema("ArrayOfObjectsAsATopPart", Description, topPart: arrayPart);
        }
    }
}

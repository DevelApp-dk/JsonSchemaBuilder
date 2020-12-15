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
    public class CodeGenerationObjectTest
    {
        [Fact]
        public void ObjectAsTopPart()
        {
            string pathString = "E:\\\\Projects\\JsonSchemaBuilder\\ModelTest\\";

            IJsonSchemaDefinition jsonSchemaDefinition = new ObjectAsTopPartJsonSchema();
            jsonSchemaDefinition.WriteSchemaToFile(pathString);
            CodeGenerator codeGenerator = new CodeGenerator();
            codeGenerator.Generate(Code.CSharp, jsonSchemaDefinition, pathString);
        }
    }

    public class ObjectAsTopPartJsonSchema : AbstractJsonSchema
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

        protected override JsonSchemaBuilderSchema BuildJsonSchema()
        {
            JsonSchemaBuilderBoolean booleanPart = new JsonSchemaBuilderBoolean("BooleanPart", "BooleanPart for testing", isRequired: true);
            JsonSchemaBuilderInteger integerPart = new JsonSchemaBuilderInteger("IntegerPart", "IntegerPart for testing");

            Dictionary<IdentifierString, IJsonSchemaBuilderPart> properties = new Dictionary<IdentifierString, IJsonSchemaBuilderPart>();
            properties.Add(booleanPart.Name, booleanPart);
            properties.Add(integerPart.Name, integerPart);

            JsonSchemaBuilderObject objectPart = new JsonSchemaBuilderObject("MyTopPartObject", "TopPart", properties: properties);

            return new JsonSchemaBuilderSchema("ObjectAsATopPart", Description, topPart: objectPart);
        }
    }
}

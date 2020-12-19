using DevelApp.JsonSchemaBuilder;
using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using DevelApp.Utility.Model;
using Manatee.Json.Schema;
using System.Collections.Generic;
using Xunit;

namespace JsonSchemaBuilder.Test
{
    public class BuildSyntaxTest
    {
        [Fact]
        public void BuildObject()
        {
            List<IJSBPart> properties = new List<IJSBPart>();
            string objectName = "ObjectName";
            string description = "ObjectDescription";
            bool isRequired = false;
            bool isExpandable = false;

            var varObject = new JSBObject(objectName, description, properties, isRequired: isRequired, isExpandable: isExpandable);
            JsonSchema varSchema = varObject.AsJsonSchema();
        }

        [Fact]
        public void BuildBoolean()
        {
            string objectName = "BooleanName";
            string description = "BooleanDescription";
            bool? defaultValue = true;
            bool isRequired = false;

            var varBoolean = new JSBBoolean(objectName, description, defaultValue, isRequired);

            JsonSchema varSchema = varBoolean.AsJsonSchema();
        }

        [Fact]
        public void BuildArray()
        {

        }

        [Fact]
        public void BuildString()
        {
            string objectName = "StringName";
            string description = "StringDescription";
            string defaultValue = "default";
            bool isRequired = false;
            uint minLength = 4;
            uint maxLength = 8;
            string pattern = "fau";

            var varString = new JSBString(objectName, description, minLength:minLength, 
                maxLength:maxLength, pattern: pattern, defaultValue: defaultValue, isRequired: isRequired);

            JsonSchema varSchema = varString.AsJsonSchema();
        }

        [Fact]
        public void BuildNumber()
        {

        }

        [Fact]
        public void BuildInteger()
        {

        }

        [Fact]
        public void BuildEnum()
        {

        }

        [Fact]
        public void BuildReference()
        {

        }

        [Fact]
        public void BuildSchema()
        {

        }

        [Fact]
        public void BuildAnyOf()
        {

        }

        [Fact]
        public void BuildOneOf()
        {

        }
    }
}

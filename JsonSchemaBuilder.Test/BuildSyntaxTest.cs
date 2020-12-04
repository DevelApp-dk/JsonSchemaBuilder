using DevelApp.JsonSchemaBuilder;
using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using System;
using System.Collections.Generic;
using Xunit;

namespace JsonSchemaBuilder.Test
{
    public class BuildSyntaxTest
    {
        [Fact]
        public void BuildObject()
        {
            List<IJsonSchemaBuilderPart> properties = new List<IJsonSchemaBuilderPart>();
            string objectName = "ObjectName";
            string description = "ObjectDescription";
            bool isRequired = false;
            bool isExpandable = false;

            var varObject = new JsonSchemaBuilderObject(objectName, description, properties, isRequired, isExpandable);
        }

        [Fact]
        public void BuildArray()
        {

        }

        [Fact]
        public void BuildString()
        {

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

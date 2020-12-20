using DevelApp.JsonSchemaBuilder;
using System;
using System.Collections.Generic;
using Xunit;
using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using DevelApp.Utility.Model;
using System.Linq;
using DevelApp.JsonSchemaBuilder.CodeGeneration;

namespace JsonSchemaBuilder.Test
{
    public class TestSchemaBuilding
    {
        [Fact]
        public void BuildNoValidation()
        {
            NoValidationJsonSchema noValidationJsonSchema = new NoValidationJsonSchema();
        }
    }
}

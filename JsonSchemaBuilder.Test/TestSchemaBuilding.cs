using DevelApp.JsonSchemaBuilder;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

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

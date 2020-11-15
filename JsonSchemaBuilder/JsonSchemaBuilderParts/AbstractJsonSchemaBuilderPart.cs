using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    public abstract class AbstractJsonSchemaBuilderPart : IJsonSchemaBuilderPart
    {
        public abstract JsonSchemaBuilderPartType PartType { get; }

        public abstract JsonSchema AsJsonSchema();
    }
}

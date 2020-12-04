using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using Manatee.Json;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder
{
    public class NoValidationJsonSchema : AbstractJsonSchema
    {
        public NoValidationJsonSchema():base(jsonSchema: JsonSchema.Empty)
        {
        }

        public override string Description
        {
            get
            {
                return "Represents an empty schema with disabled validation";
            }
        }

        public override string Module
        {
            get
            {
                return "Default";
            }
        }

        protected override JsonSchemaBuilderSchema BuildJsonSchema()
        {
            throw new NotImplementedException($"BuildJsonSchema should not be called as JsonSchema has been provided");
        }
    }
}
